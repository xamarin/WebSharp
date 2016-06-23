/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_time.idl modified Thu May 12 06:59:59 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines time, time ticks and time delta types.
 */


/**
 * @addtogroup Typedefs
 * @{
 */
/**
 * The <code>PP_Time</code> type represents the "wall clock time" according
 * to the browser and is defined as the number of seconds since the Epoch
 * (00:00:00 UTC, January 1, 1970).
 */
[StructLayout(LayoutKind.Sequential)]
public struct PP_Time {
	public double pp_time;
}

/**
 * A <code>PP_TimeTicks</code> value represents time ticks which are measured
 * in seconds and are used for indicating the time that certain messages were
 * received. In contrast to <code>PP_Time</code>, <code>PP_TimeTicks</code>
 * does not correspond to any actual wall clock time and will not change
 * discontinuously if the user changes their computer clock.
 *
 * The units are in seconds, but are not measured relative to any particular
 * epoch, so the most you can do is compare two values.
 */
[StructLayout(LayoutKind.Sequential)]
public struct PP_TimeTicks {
	public double pp_timeticks;
}

/**
 * A <code>PP_TimeDelta</code> value represents a duration of time which is
 * measured in seconds.
 */
[StructLayout(LayoutKind.Sequential)]
public struct PP_TimeDelta {
	public double pp_timedelta;
}
/**
 * @}
 */


}
