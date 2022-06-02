using System.Collections;
using System.Collections.Generic;
using System;

public class InvalidSpeakerException : Exception
{
    public InvalidSpeakerException(string speakerName) : base(String.Format("Invalid Speaker detected. Current InvalidSpeaker Name: {0}", speakerName)){
    }
}
