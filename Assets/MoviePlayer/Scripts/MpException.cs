//--------------------------------------------
// Movie Player
// Copyright © 2014 SHUU Games
//--------------------------------------------
using System;
using System.Runtime.Serialization;

namespace MP
{
	#region ----- Exceptions -----
	public class MpException : ApplicationException
	{
		public MpException () : base()
		{
		}
		
		public MpException (string msg) : base(msg)
		{
		}
		
		public MpException (string msg, System.Exception inner) : base(msg, inner)
		{
		}

		#if !UNITY_WINRT
		public MpException (SerializationInfo info, StreamingContext ctx) : base(info, ctx)
		{
		}
		#endif
	}
	#endregion
}
