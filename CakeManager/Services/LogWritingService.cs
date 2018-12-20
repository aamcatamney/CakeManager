using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CakeManager.Services
{
  public interface ILogWritingService
  {
    void WriteToLog(string Path, string Contents);
  }

  public class LogWritingService : ILogWritingService
  {
    private volatile bool writting = false;
    private volatile Queue<WriteRequest> queue = new Queue<WriteRequest>();
    public void WriteToLog(string Path, string Contents)
    {
      queue.Enqueue(new WriteRequest() { Path = Path, Contents = Contents });
      Task.Run(() => Write());
    }

    private void Write()
    {
      if (!writting)
      {
        if (!writting)
        {
          writting = true;
          if (queue.Count > 0)
          {
            var i = queue.Dequeue();
            File.AppendAllLines(i.Path, new[] { i.Contents });
          }
          writting = false;
          if (queue.Count > 0)
          {
            Write();
          }
        }
      }
    }

    private class WriteRequest
    {
      public string Path { get; set; }
      public string Contents { get; set; }
    }
  }
}