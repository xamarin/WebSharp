/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_console.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_Console</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
public enum PPLogLevel {
  Tip = 0,
  Log = 1,
  Warning = 2,
  Error = 3
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
internal static partial class PPBConsole {
  [DllImport("PepperPlugin", EntryPoint = "PPB_Console_Log")]
  extern static void _Log ( PPInstance instance,
                            PPLogLevel level,
                            PPVar value);

  /**
   * Logs the given message to the JavaScript console associated with the
   * given plugin instance with the given logging level. The name of the plugin
   * issuing the log message will be automatically prepended to the message.
   * The value may be any type of Var.
   */
  public static void Log ( PPInstance instance,
                           PPLogLevel level,
                           PPVar value)
  {
  	 _Log (instance, level, value);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Console_LogWithSource")]
  extern static void _LogWithSource ( PPInstance instance,
                                      PPLogLevel level,
                                      PPVar source,
                                      PPVar value);

  /**
   * Logs a message to the console with the given source information rather
   * than using the internal PPAPI plugin name. The name must be a string var.
   *
   * The regular log function will automatically prepend the name of your
   * plugin to the message as the "source" of the message. Some plugins may
   * wish to override this. For example, if your plugin is a Python
   * interpreter, you would want log messages to contain the source .py file
   * doing the log statement rather than have "python" show up in the console.
   */
  public static void LogWithSource ( PPInstance instance,
                                     PPLogLevel level,
                                     PPVar source,
                                     PPVar value)
  {
  	 _LogWithSource (instance, level, source, value);
  }


}
/**
 * @}
 */


}
