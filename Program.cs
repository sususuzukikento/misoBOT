// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
using System;
using System.IO;
using System.Text.Json; // Json処理
using System.Speech.Synthesis; // 音声合成
using NAudio.Wave; // NAudioのWave機能
using Vosk; // Vosk音声認識ライブラリ
using NAudio.CoreAudioApi;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
  static void Main(string[] args)
  {
    const string modelPath = "vosk-model-ja-0.22";

    if (!Directory.Exists(modelPath))
    {
      Console.WriteLine($"モデルが見つかりません: {modelPath}");
      return;
    }

    Vosk.Vosk.SetLogLevel(0);
    var model = new Model(modelPath);

    using (var waveIn = new WaveInEvent())
    {
      waveIn.WaveFormat = new WaveFormat(16000, 1);
      var recognizer = new VoskRecognizer(model, 16000.0f);

      waveIn.DataAvailable += (sender, e) =>
      {
        if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
        {
          string resultJson = recognizer.Result();
          var result = ParseJsonResult(resultJson);

          if (!string.IsNullOrWhiteSpace(result))
          {
            Console.WriteLine($"認識結果: {result}");
            SpeakText(result, "CABLE Input (VB-Audio Virtual Cable)"); // 仮想デバに出力
          }
        }
        else
        {
          Console.WriteLine(recognizer.PartialResult());
        }
      };

      Console.WriteLine("音声認識を開始します。Ctrl+Cで終了してください。");
      waveIn.StartRecording();
      Console.ReadLine();
      waveIn.StopRecording();
    }
  }

  static string ParseJsonResult(string json)
  {
    try
    {
      using (var doc = JsonDocument.Parse(json))
      {
        if (doc.RootElement.TryGetProperty("text", out var textElement))
        {
          return textElement.GetString();
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"JSON解析エラー: {ex.Message}");
    }
    return string.Empty;
  }

  static void SpeakText(string text, string outputDevice)
  {
    try
    {
      using (var synth = new SpeechSynthesizer())
      using (var waveStream = new MemoryStream())
      using (var waveOut = new WaveOutEvent { DeviceNumber = GetDeviceIndex(outputDevice) })
      {
        synth.SetOutputToWaveStream(waveStream);
        synth.Speak(text);
        waveStream.Position = 0;

        var waveProvider = new WaveFileReader(waveStream);
        waveOut.Init(waveProvider);
        waveOut.Play();

        while (waveOut.PlaybackState == PlaybackState.Playing)
        {
          System.Threading.Thread.Sleep(100);
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"音声合成エラー: {ex.Message}");
    }
  }

  static int GetDeviceIndex(string deviceName)
  {
    var enumerator = new MMDeviceEnumerator();
    var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

    for (int i = 0; i < devices.Count; i++)
    {
      if (devices[i].FriendlyName == deviceName)
      {
        return i;
      }
    }
    throw new Exception($"デバイス '{deviceName}' が見つかりません。");
  }
}