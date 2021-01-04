using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.Speech
{
    public delegate void SpeechRecognizedDelegate(string input);

    public interface ISpeechService
    {
        void StartRecording();

        event SpeechRecognizedDelegate OnSpeechRecognized;
    }
}
