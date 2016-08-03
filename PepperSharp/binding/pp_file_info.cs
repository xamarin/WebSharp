/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_file_info.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines three enumerations for use in the PPAPI C file IO APIs.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * The <code>PP_FileType</code> enum contains file type constants.
 */
public enum PPFileType {
  /** A regular file type */
  Regular = 0,
  /** A directory */
  Directory = 1,
  /** A catch-all for unidentified types */
  Other = 2
}

/**
 * The <code>PP_FileSystemType</code> enum contains file system type constants.
 */
public enum PPFileSystemType {
  /** For identified invalid return values */
  Invalid = 0,
  /** For external file system types */
  External = 1,
  /** For local persistent file system types */
  Localpersistent = 2,
  /** For local temporary file system types */
  Localtemporary = 3,
  /** For isolated file system types */
  Isolated = 4
}
/**
 * @}
 */

/**
 * @addtogroup Structs
 * @{
 */
/**
 * The <code>PP_FileInfo</code> struct represents all information about a file,
 * such as size, type, and creation time.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPFileInfo {
  /** This value represents the size of the file measured in bytes */
  public long size;
  /**
   * This value represents the type of file as defined by the
   * <code>PP_FileType</code> enum
   */
  public PPFileType type;
  /**
   * This value represents the file system type of the file as defined by the
   * <code>PP_FileSystemType</code> enum.
   */
  public PPFileSystemType system_type;
  /**
   * This value represents the creation time of the file.
   */
  public double creation_time;
  /**
   * This value represents the last time the file was accessed.
   */
  public double last_access_time;
  /**
   * This value represents the last time the file was modified.
   */
  public double last_modified_time;
}
/**
 * @}
 */


}
