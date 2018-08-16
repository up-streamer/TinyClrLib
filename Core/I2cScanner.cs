﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using GHIElectronics.TinyCLR.Devices.I2c;
// ReSharper disable UnusedMember.Global

namespace TinyClrCore
{
    public static class I2CScanner
    {
        public static ArrayList Ping(string filter)
        {
            var i2CCtl = I2cController.FromName(filter);
            ArrayList list = new ArrayList();
            byte[] readBuffer = new byte[7];
            byte[] writeBuffer = new byte[1];
            for (int i = 0x03; i < 0x77; i++)
            {
                I2cConnectionSettings settings = new I2cConnectionSettings(i,I2cBusSpeed.FastMode);
                I2cDevice device = i2CCtl.GetDevice(settings);
                for (int j = 0; j < readBuffer.Length; j++)
                {
                    readBuffer[j] = 0;
                }
                try
                {
                    writeBuffer[0] = 0x00;
                    var res = device.WriteReadPartial(writeBuffer, readBuffer);
                    // Debug.WriteLine("Device #" + i.ToString("X") + ": " + DisplayReadBuffer(readBuffer) + ", status:" + DisplayStatus(res.Status));
                    if (res.Status == I2cTransferStatus.FullTransfer)
                        list.Add(i);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    device.Dispose();
                }
                Thread.Sleep(10);
            }
            return list;
        }

        //private static string DisplayStatus(I2cTransferStatus resStatus)
        //{
        //    string str = "";
        //    switch (resStatus)
        //    {
        //        case I2cTransferStatus.FullTransfer:
        //            str = "FullTransfert";
        //            break;
        //        case I2cTransferStatus.PartialTransfer:
        //            str = "PartialTransfer";
        //            break;
        //        case I2cTransferStatus.SlaveAddressNotAcknowledged:
        //            str = "Slave NACK";
        //            break;
        //    }
        //    return str;
        //}

        //private static string DisplayReadBuffer(byte[] readBuffer)
        //{
        //    string str = "";
        //    foreach (var value in readBuffer)
        //    {
        //        str += value.ToString("X");
        //    }
        //    return str;
        //}
    }

}