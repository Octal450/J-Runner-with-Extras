using Microsoft.Win32.SafeHandles;
using System;

namespace WinUsb
{
    /// <summary>
    ///  Routines for the WinUsb driver supported by Windows Vista and Windows XP.
    ///  </summary>
    ///  
    sealed internal partial class WinUsbDevice
    {
        internal struct devInfo
        {
            internal SafeFileHandle deviceHandle;
            internal IntPtr winUsbHandle;
            internal Byte bulkInPipe;
            internal Byte bulkOutPipe;
            internal Byte interruptInPipe;
            internal Byte interruptOutPipe;
            internal UInt32 devicespeed;
        }

        internal devInfo myDevInfo = new devInfo();

        ///  <summary>
        ///  Closes the device handle obtained with CreateFile and frees resources.
        ///  </summary>
        ///  
        internal void CloseDeviceHandle()
        {
            try
            {
                NativeMethods.WinUsb_Free(myDevInfo.winUsbHandle);

                if (!(myDevInfo.deviceHandle == null))
                {
                    if (!(myDevInfo.deviceHandle.IsInvalid))
                    {
                        myDevInfo.deviceHandle.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Initiates a Control Read transfer. Data stage is device to host.
        ///  </summary>
        /// 
        ///  <param name="dataStage"> The received data. </param>
        ///  
        ///  <returns>
        ///  True on success, False on failure.
        ///  </returns>

        internal Boolean Do_Control_Read_Transfer(ref Byte[] dataStage)
        {
            UInt32 bytesReturned = 0;
            NativeMethods.WINUSB_SETUP_PACKET setupPacket;
            Boolean success;

            try
            {
                //  Vendor-specific request to an interface with device-to-host Data stage.

                setupPacket.RequestType = 0XC1;

                //  The request number that identifies the specific request.

                setupPacket.Request = 2;

                //  Command-specific value to send to the device.

                setupPacket.Index = 0;

                //  Number of bytes in the request's Data stage.

                setupPacket.Length = System.Convert.ToUInt16(dataStage.Length);

                //  Command-specific value to send to the device.

                setupPacket.Value = 0;

                // ***
                //  winusb function 

                //  summary
                //  Initiates a control transfer.

                //  paramaters
                //  Device handle returned by WinUsb_Initialize.
                //  WINUSB_SETUP_PACKET structure 
                //  Buffer to hold the returned Data-stage data.
                //  Number of data bytes to read in the Data stage.
                //  Number of bytes read in the Data stage.
                //  Null pointer for non-overlapped.

                //  returns
                //  True on success.
                //  ***            

                success = NativeMethods.WinUsb_ControlTransfer(myDevInfo.winUsbHandle, setupPacket, dataStage, System.Convert.ToUInt16(dataStage.Length), ref bytesReturned, IntPtr.Zero);
                return success;

            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Initiates a Control Write transfer. Data stage is host to device.
        ///  </summary>
        ///  
        ///  <param name="dataStage"> The data to send. </param>
        ///  
        ///  <returns>
        ///  True on success, False on failure.
        ///  </returns>

        internal Boolean Do_Control_Write_Transfer(Byte[] dataStage)
        {
            UInt32 bytesReturned = 0;
            ushort index = System.Convert.ToUInt16(0);
            NativeMethods.WINUSB_SETUP_PACKET setupPacket;
            Boolean success;
            ushort value = System.Convert.ToUInt16(0);

            try
            {
                //  Vendor-specific request to an interface with host-to-device Data stage.

                setupPacket.RequestType = 0X41;

                //  The request number that identifies the specific request.

                setupPacket.Request = 1;

                //  Command-specific value to send to the device.

                setupPacket.Index = index;

                //  Number of bytes in the request's Data stage.

                setupPacket.Length = System.Convert.ToUInt16(dataStage.Length);

                //  Command-specific value to send to the device.

                setupPacket.Value = value;

                // ***
                //  winusb function 

                //  summary
                //  Initiates a control transfer.

                //  parameters
                //  Device handle returned by WinUsb_Initialize.
                //  WINUSB_SETUP_PACKET structure 
                //  Buffer containing the Data-stage data.
                //  Number of data bytes to send in the Data stage.
                //  Number of bytes sent in the Data stage.
                //  Null pointer for non-overlapped.

                //  Returns
                //  True on success.
                //  ***

                success = NativeMethods.WinUsb_ControlTransfer
                    (myDevInfo.winUsbHandle,
                    setupPacket,
                    dataStage,
                    System.Convert.ToUInt16(dataStage.Length),
                    ref bytesReturned,
                    IntPtr.Zero);
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Requests a handle with CreateFile.
        ///  </summary>
        ///  
        ///  <param name="devicePathName"> Returned by SetupDiGetDeviceInterfaceDetail 
        ///  in an SP_DEVICE_INTERFACE_DETAIL_DATA structure. </param>
        ///  
        ///  <returns>
        ///  The handle.
        ///  </returns>

        internal Boolean GetDeviceHandle(String devicePathName)
        {
            // ***
            // API function

            //  summary
            //  Retrieves a handle to a device.

            //  parameters 
            //  Device path name returned by SetupDiGetDeviceInterfaceDetail
            //  Type of access requested (read/write).
            //  FILE_SHARE attributes to allow other processes to access the device while this handle is open.
            //  Security structure. Using Null for this may cause problems under Windows XP.
            //  Creation disposition value. Use OPEN_EXISTING for devices.
            //  Flags and attributes for files. The winsub driver requires FILE_FLAG_OVERLAPPED.
            //  Handle to a template file. Not used.

            //  Returns
            //  A handle or INVALID_HANDLE_VALUE.
            // ***

            myDevInfo.deviceHandle = FileIO.CreateFile
                (devicePathName,
                (FileIO.GENERIC_WRITE | FileIO.GENERIC_READ),
                FileIO.FILE_SHARE_READ | FileIO.FILE_SHARE_WRITE,
                IntPtr.Zero,
                FileIO.OPEN_EXISTING,
                FileIO.FILE_ATTRIBUTE_NORMAL | FileIO.FILE_FLAG_OVERLAPPED,
                0);

            if (!(myDevInfo.deviceHandle.IsInvalid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        ///  <summary>
        ///  Initializes a device interface and obtains information about it.
        ///  Calls these winusb API functions:
        ///    WinUsb_Initialize
        ///    WinUsb_QueryInterfaceSettings
        ///    WinUsb_QueryPipe
        ///  </summary>
        ///  
        ///  <param name="deviceHandle"> A handle obtained in a call to winusb_initialize. </param>
        ///  
        ///  <returns>
        ///  True on success, False on failure.
        ///  </returns>

        internal Boolean InitializeDevice()
        {
            NativeMethods.USB_INTERFACE_DESCRIPTOR ifaceDescriptor;
            NativeMethods.WINUSB_PIPE_INFORMATION pipeInfo;
            UInt32 pipeTimeout = 2000;
            Boolean success;

            try
            {
                ifaceDescriptor.bLength = 0;
                ifaceDescriptor.bDescriptorType = 0;
                ifaceDescriptor.bInterfaceNumber = 0;
                ifaceDescriptor.bAlternateSetting = 0;
                ifaceDescriptor.bNumEndpoints = 0;
                ifaceDescriptor.bInterfaceClass = 0;
                ifaceDescriptor.bInterfaceSubClass = 0;
                ifaceDescriptor.bInterfaceProtocol = 0;
                ifaceDescriptor.iInterface = 0;

                pipeInfo.PipeType = 0;
                pipeInfo.PipeId = 0;
                pipeInfo.MaximumPacketSize = 0;
                pipeInfo.Interval = 0;

                // ***
                //  winusb function 

                //  summary
                //  get a handle for communications with a winusb device        '

                //  parameters
                //  Handle returned by CreateFile.
                //  Device handle to be returned.

                //  returns
                //  True on success.
                //  ***

                success = NativeMethods.WinUsb_Initialize
                    (myDevInfo.deviceHandle,
                    ref myDevInfo.winUsbHandle);

                if (success)
                {
                    // ***
                    //  winusb function 

                    //  summary
                    //  Get a structure with information about the device interface.

                    //  parameters
                    //  handle returned by WinUsb_Initialize
                    //  alternate interface setting number
                    //  USB_INTERFACE_DESCRIPTOR structure to be returned.

                    //  returns
                    //  True on success.

                    success = NativeMethods.WinUsb_QueryInterfaceSettings
                        (myDevInfo.winUsbHandle,
                        0,
                        ref ifaceDescriptor);

                    if (success)
                    {
                        //  Get the transfer type, endpoint number, and direction for the interface's
                        //  bulk and interrupt endpoints. Set pipe policies.

                        // ***
                        //  winusb function 

                        //  summary
                        //  returns information about a USB pipe (endpoint address)

                        //  parameters
                        //  Handle returned by WinUsb_Initialize
                        //  Alternate interface setting number
                        //  Number of an endpoint address associated with the interface. 
                        //  (The values count up from zero and are NOT the same as the endpoint address
                        //  in the endpoint descriptor.)
                        //  WINUSB_PIPE_INFORMATION structure to be returned

                        //  returns
                        //  True on success   
                        // ***

                        for (Int32 i = 0; i <= ifaceDescriptor.bNumEndpoints - 1; i++)
                        {
                            NativeMethods.WinUsb_QueryPipe
                                (myDevInfo.winUsbHandle,
                                0,
                                System.Convert.ToByte(i),
                                ref pipeInfo);

                            if (((pipeInfo.PipeType ==
                                NativeMethods.USBD_PIPE_TYPE.UsbdPipeTypeBulk) &
                                UsbEndpointDirectionIn(pipeInfo.PipeId)))
                            {
                                myDevInfo.bulkInPipe = pipeInfo.PipeId;

                                SetPipePolicy
                                    (myDevInfo.bulkInPipe,
                                    Convert.ToUInt32(POLICY_TYPE.IGNORE_SHORT_PACKETS),
                                    Convert.ToByte(false));

                                SetPipePolicy
                                    (myDevInfo.bulkInPipe,
                                    Convert.ToUInt32(POLICY_TYPE.PIPE_TRANSFER_TIMEOUT),
                                    pipeTimeout);

                            }
                            else if (((pipeInfo.PipeType ==
                                NativeMethods.USBD_PIPE_TYPE.UsbdPipeTypeBulk) &
                                UsbEndpointDirectionOut(pipeInfo.PipeId)))
                            {

                                myDevInfo.bulkOutPipe = pipeInfo.PipeId;

                                SetPipePolicy
                                    (myDevInfo.bulkOutPipe,
                                    Convert.ToUInt32(POLICY_TYPE.IGNORE_SHORT_PACKETS),
                                    Convert.ToByte(false));

                                SetPipePolicy
                                    (myDevInfo.bulkOutPipe,
                                    Convert.ToUInt32(POLICY_TYPE.PIPE_TRANSFER_TIMEOUT),
                                    pipeTimeout);

                            }
                            else if ((pipeInfo.PipeType ==
                                NativeMethods.USBD_PIPE_TYPE.UsbdPipeTypeInterrupt) &
                                UsbEndpointDirectionIn(pipeInfo.PipeId))
                            {

                                myDevInfo.interruptInPipe = pipeInfo.PipeId;

                                SetPipePolicy
                                    (myDevInfo.interruptInPipe,
                                    Convert.ToUInt32(POLICY_TYPE.IGNORE_SHORT_PACKETS),
                                    Convert.ToByte(false));

                                SetPipePolicy
                                    (myDevInfo.interruptInPipe,
                                    Convert.ToUInt32(POLICY_TYPE.PIPE_TRANSFER_TIMEOUT),
                                    pipeTimeout);

                            }
                            else if ((pipeInfo.PipeType ==
                                NativeMethods.USBD_PIPE_TYPE.UsbdPipeTypeInterrupt) &
                                UsbEndpointDirectionOut(pipeInfo.PipeId))
                            {

                                myDevInfo.interruptOutPipe = pipeInfo.PipeId;

                                SetPipePolicy
                                    (myDevInfo.interruptOutPipe,
                                    Convert.ToUInt32(POLICY_TYPE.IGNORE_SHORT_PACKETS),
                                    Convert.ToByte(false));

                                SetPipePolicy
                                    (myDevInfo.interruptOutPipe,
                                    Convert.ToUInt32(POLICY_TYPE.PIPE_TRANSFER_TIMEOUT),
                                    pipeTimeout);
                            }
                        }
                    }
                    else
                    {
                        success = false;
                    }
                }
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Is the current operating system Windows XP or later?
        ///  The WinUSB driver requires Windows XP or later.
        ///  </summary>
        /// 
        ///  <returns>
        ///  True if Windows XP or later, False if not.
        ///  </returns>

        internal Boolean IsWindowsXpOrLater()
        {
            try
            {
                OperatingSystem myEnvironment = Environment.OSVersion;

                //  Windows XP is version 5.1.

                System.Version versionXP = new System.Version(5, 1);

                if (myEnvironment.Version >= versionXP)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Gets a value that corresponds to a USB_DEVICE_SPEED. 
        ///  </summary>

        internal Boolean QueryDeviceSpeed()
        {
            UInt32 length = 1;
            Byte[] speed = new Byte[1];
            Boolean success;

            // ***
            //  winusb function 

            //  summary
            //  Get the device speed. 
            //  (Normally not required but can be nice to know.)

            //  parameters
            //  Handle returned by WinUsb_Initialize
            //  Requested information type.
            //  Number of bytes to read.
            //  Information to be returned.

            //  returns
            //  True on success.
            // ***           

            success = NativeMethods.WinUsb_QueryDeviceInformation
                (myDevInfo.winUsbHandle,
                DEVICE_SPEED,
                ref length,
                ref speed[0]);

            if (success)
            {
                myDevInfo.devicespeed = System.Convert.ToUInt32(speed[0]);
            }

            return success;
        }

        ///  <summary>
        ///  Attempts to read data from a bulk IN endpoint.
        ///  </summary>
        ///  
        ///  <param name="InterfaceHandle"> Device interface handle. </param>
        ///  <param name="PipeID"> Endpoint address. </param>
        ///  <param name="bytesToRead"> Number of bytes to read. </param>
        ///  <param name="Buffer"> Buffer for storing the bytes read. </param>
        ///  <param name="bytesRead"> Number of bytes read. </param>
        ///  <param name="success"> Success or failure status. </param>
        ///  
        internal void ReadViaBulkTransfer(Byte pipeID, UInt32 bytesToRead, ref Byte[] buffer, ref UInt32 bytesRead, ref Boolean success)
        {
            try
            {
                // ***
                //  winusb function 

                //  summary
                //  Attempts to read data from a device interface.

                //  parameters
                //  Device handle returned by WinUsb_Initialize.
                //  Endpoint address.
                //  Buffer to store the data.
                //  Maximum number of bytes to return.
                //  Number of bytes read.
                //  Null pointer for non-overlapped.

                //  Returns
                //  True on success.
                // ***

                success = NativeMethods.WinUsb_ReadPipe
                    (myDevInfo.winUsbHandle,
                    pipeID,
                    buffer,
                    bytesToRead,
                    ref bytesRead,
                    IntPtr.Zero);

                if (!(success))
                {
                    CloseDeviceHandle();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Attempts to read data from an interrupt IN endpoint. 
        ///  </summary>
        ///  
        ///  <param name="InterfaceHandle"> Device interface handle. </param>
        ///  <param name="PipeID"> Endpoint address. </param>
        ///  <param name="bytesToRead"> Number of bytes to read. </param>
        ///  <param name="Buffer"> Buffer for storing the bytes read. </param>
        ///  <param name="bytesRead"> Number of bytes read. </param>
        ///  <param name="success"> Success or failure status. </param>
        ///  
        internal void ReadViaInterruptTransfer
            (Byte pipeID,
            UInt32 bytesToRead,
            ref Byte[] buffer,
            ref UInt32 bytesRead,
            ref Boolean success)
        {
            try
            {
                // ***
                //  winusb function 

                //  summary
                //  Attempts to read data from a device interface.

                //  parameters
                //  Device handle returned by WinUsb_Initialize.
                //  Endpoint address.
                //  Buffer to store the data.
                //  Maximum number of bytes to return.
                //  Number of bytes read.
                //  Null pointer for non-overlapped.

                //  Returns
                //  True on success.
                // ***

                success = NativeMethods.WinUsb_ReadPipe
                    (myDevInfo.winUsbHandle,
                    pipeID,
                    buffer,
                    bytesToRead,
                    ref bytesRead,
                    IntPtr.Zero);

                if (!(success))
                {
                    CloseDeviceHandle();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Attempts to send data via a bulk OUT endpoint.
        ///  </summary>
        ///  
        ///  <param name="buffer"> Buffer containing the bytes to write. </param>
        ///  <param name="bytesToWrite"> Number of bytes to write. </param>
        ///  
        ///  <returns>
        ///  True on success, False on failure.
        ///  </returns>

        internal Boolean SendViaBulkTransfer(ref Byte[] buffer, UInt32 bytesToWrite)
        {
            UInt32 bytesWritten = 0;
            Boolean success;

            try
            {
                // ***
                //  winusb function 

                //  summary
                //  Attempts to write data to a device interface.

                //  parameters
                //  Device handle returned by WinUsb_Initialize.
                //  Endpoint address.
                //  Buffer with data to write.
                //  Number of bytes to write.
                //  Number of bytes written.
                //  IntPtr.Zero for non-overlapped I/O.

                //  Returns
                //  True on success.
                //  ***

                success = NativeMethods.WinUsb_WritePipe
                    (myDevInfo.winUsbHandle,
                    myDevInfo.bulkOutPipe,
                    buffer,
                    bytesToWrite,
                    ref bytesWritten,
                    IntPtr.Zero);

                if (!(success))
                {
                    CloseDeviceHandle();
                }
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Attempts to send data via an interrupt OUT endpoint.
        ///  </summary>
        ///  
        ///  <param name="buffer"> Buffer containing the bytes to write. </param>
        ///  <param name="bytesToWrite"> Number of bytes to write. </param>
        ///  
        ///  <returns>
        ///  True on success, False on failure.
        ///  </returns>

        internal Boolean SendViaInterruptTransfer(ref Byte[] buffer, UInt32 bytesToWrite)
        {
            UInt32 bytesWritten = 0;
            Boolean success;

            try
            {
                // ***
                //  winusb function 

                //  summary
                //  Attempts to write data to a device interface.

                //  parameters
                //  Device handle returned by WinUsb_Initialize.
                //  Endpoint address.
                //  Buffer with data to write.
                //  Number of bytes to write.
                //  Number of bytes written.
                //  IntPtr.Zero for non-overlapped I/O.

                //  Returns
                //  True on success.
                //  ***

                success = NativeMethods.WinUsb_WritePipe
                    (myDevInfo.winUsbHandle,
                    myDevInfo.interruptOutPipe,
                    buffer,
                    bytesToWrite,
                    ref bytesWritten,
                    IntPtr.Zero);

                if (!(success))
                {
                    CloseDeviceHandle();
                }

                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Sets pipe policy.
        ///  Used when the value parameter is a Byte (all except PIPE_TRANSFER_TIMEOUT).
        ///  </summary>
        ///  
        ///  <param name="pipeId"> Pipe to set a policy for. </param>
        ///  <param name="policyType"> POLICY_TYPE member. </param>
        ///  <param name="value"> Policy value. </param>
        ///  
        ///  <returns>
        ///  True on success, False on failure.
        ///  </returns>
        ///  
        private Boolean SetPipePolicy(Byte pipeId, UInt32 policyType, Byte value)
        {
            Boolean success;

            try
            {
                // ***
                //  winusb function 

                //  summary
                //  sets a pipe policy 

                //  parameters
                //  handle returned by WinUsb_Initialize
                //  identifies the pipe
                //  POLICY_TYPE member.
                //  length of value in bytes
                //  value to set for the policy.

                //  returns
                //  True on success 
                // ***

                success = NativeMethods.WinUsb_SetPipePolicy
                    (myDevInfo.winUsbHandle,
                    pipeId,
                    policyType,
                    1,
                    ref value);

                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Sets pipe policy.
        ///  Used when the value parameter is a UInt32 (PIPE_TRANSFER_TIMEOUT only).
        ///  </summary>
        ///  
        ///  <param name="pipeId"> Pipe to set a policy for. </param>
        ///  <param name="policyType"> POLICY_TYPE member. </param>
        ///  <param name="value"> Policy value. </param>
        ///  
        ///  <returns>
        ///  True on success, False on failure.
        ///  </returns>
        ///  
        private Boolean SetPipePolicy(Byte pipeId, UInt32 policyType, UInt32 value)
        {
            Boolean success;

            try
            {
                // ***
                //  winusb function 

                //  summary
                //  sets a pipe policy 

                //  parameters
                //  handle returned by WinUsb_Initialize
                //  identifies the pipe
                //  POLICY_TYPE member.
                //  length of value in bytes
                //  value to set for the policy.

                //  returns
                //  True on success 
                // ***

                success = NativeMethods.WinUsb_SetPipePolicy1
                    (myDevInfo.winUsbHandle,
                    pipeId,
                    policyType,
                    4,
                    ref value);

                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///  <summary>
        ///  Is the endpoint's direction IN (device to host)?
        ///  </summary>
        ///  
        ///  <param name="addr"> The endpoint address. </param>
        ///  <returns>
        ///  True if IN (device to host), False if OUT (host to device)
        ///  </returns> 

        private Boolean UsbEndpointDirectionIn(Int32 addr)
        {
            Boolean directionIn;

            try
            {
                if (((addr & 0X80) == 0X80))
                {
                    directionIn = true;
                }
                else
                {
                    directionIn = false;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return directionIn;
        }


        ///  <summary>
        ///  Is the endpoint's direction OUT (host to device)?
        ///  </summary>
        ///  
        ///  <param name="addr"> The endpoint address. </param>
        ///  
        ///  <returns>
        ///  True if OUT (host to device, False if IN (device to host)
        ///  </returns>

        private Boolean UsbEndpointDirectionOut(Int32 addr)
        {
            Boolean directionOut;

            try
            {
                if (((addr & 0X80) == 0))
                {
                    directionOut = true;
                }
                else
                {
                    directionOut = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return directionOut;
        }
    }
}
