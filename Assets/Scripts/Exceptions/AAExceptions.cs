using System;

public class AAExceptions
{
    public class ServiceIsUninitialized : Exception
    {
        public ServiceIsUninitialized(string message)
            : base(message)
        { }

    }

    public class ImportentComponentNotFound : Exception
    {
        public ImportentComponentNotFound(string message)
            : base(message)
        { }

    }
}
