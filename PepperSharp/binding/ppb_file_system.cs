/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_file_system.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the API to create a file system associated with a file.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_FileSystem</code> struct identifies the file system type
 * associated with a file.
 */
public static partial class PPBFileSystem {
  [DllImport("PepperPlugin", EntryPoint = "PPB_FileSystem_Create")]
  extern static PPResource _Create ( PPInstance instance,
                                     PPFileSystemType type);

  /** Create() creates a file system object of the given type.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying the instance
   * with the file.
   * @param[in] type A file system type as defined by
   * <code>PP_FileSystemType</code> enum (except PP_FILESYSTEMTYPE_ISOLATED,
   * which is currently not supported).
   * @return A <code>PP_Resource</code> corresponding to a file system if
   * successful.
   */
  public static PPResource Create ( PPInstance instance,
                                    PPFileSystemType type)
  {
  	return _Create (instance, type);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileSystem_IsFileSystem")]
  extern static PPBool _IsFileSystem ( PPResource resource);

  /**
   * IsFileSystem() determines if the provided resource is a file system.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a file
   * system.
   *
   * @return <code>PP_TRUE</code> if the resource is a
   * <code>PPB_FileSystem</code>, <code>PP_FALSE</code> if the resource is
   * invalid or some type other than <code>PPB_FileSystem</code>.
   */
  public static PPBool IsFileSystem ( PPResource resource)
  {
  	return _IsFileSystem (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileSystem_Open")]
  extern static int _Open ( PPResource file_system,
                            long expected_size,
                            PPCompletionCallback callback);

  /**
   * Open() opens the file system. A file system must be opened before running
   * any other operation on it.
   *
   * @param[in] file_system A <code>PP_Resource</code> corresponding to a file
   * system.
   *
   * @param[in] expected_size The expected size of the file system. Note that
   * this does not request quota; to do that, you must either invoke
   * requestQuota from JavaScript:
   * http://www.html5rocks.com/en/tutorials/file/filesystem/#toc-requesting-quota
   * or set the unlimitedStorage permission for Chrome Web Store apps:
   * http://code.google.com/chrome/extensions/manifest.html#permissions
   *
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of Open().
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int Open ( PPResource file_system,
                           long expected_size,
                           PPCompletionCallback callback)
  {
  	return _Open (file_system, expected_size, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileSystem_GetType")]
  extern static PPFileSystemType _GetType ( PPResource file_system);

  /**
   * GetType() returns the type of the provided file system.
   *
   * @param[in] file_system A <code>PP_Resource</code> corresponding to a file
   * system.
   *
   * @return A <code>PP_FileSystemType</code> with the file system type if
   * valid or <code>PP_FILESYSTEMTYPE_INVALID</code> if the provided resource
   * is not a valid file system. It is valid to call this function even before
   * Open() completes.
   */
  public static PPFileSystemType GetType ( PPResource file_system)
  {
  	return _GetType (file_system);
  }


}
/**
 * @}
 */


}
