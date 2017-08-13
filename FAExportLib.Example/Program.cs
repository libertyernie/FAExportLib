using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAExportLib.Example {
    class Program {
        static void Main(string[] args) {
            GetUser().GetAwaiter().GetResult();
        }

        static async Task GetUser() {
            FAClient client = new FAClient();
            var user = await client.GetUserAsync("lizard-socks");
            Console.WriteLine(user.name);
            foreach (var ci in user.contact_information) {
                Console.WriteLine($"{ci.title}: {ci.name}");
            }
            foreach (var u in user.watchers.recent) {
                Console.WriteLine("Watched by " + u.name);
            }
        }
    }
}
