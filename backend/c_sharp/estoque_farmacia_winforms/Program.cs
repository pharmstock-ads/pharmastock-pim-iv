using estoque_farmacia_winforms.Forms;

namespace estoque_farmacia_winforms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}
