using System;

namespace MC.Project.UI
{
    internal enum ErrorCode
    {
        DuplicatePath,
        InvalidPath,
        NoComponentInPath,
        DuplicateComponent,
        Unknown,
    }
        
    internal class UIException : Exception
    {
        public UIException( ErrorCode code ) : base( code.ToString() )
        {
            
        }
        
        public UIException( ErrorCode code, string subMessage ) : base( code + ", " + subMessage )
        {
            
        }
    }
}