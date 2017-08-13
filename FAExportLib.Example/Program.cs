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
            Console.Write("Enter a FurAffinity username (default: lizard-socks): ");
            string username = Console.ReadLine();
            if (string.IsNullOrEmpty(username)) username = "lizard-socks";

            var user = await client.GetUserAsync(username);
            Console.WriteLine(user.name);
            if (user.contact_information != null) {
                foreach (var ci in user.contact_information) {
                    Console.WriteLine($"{ci.title}: {ci.name}");
                }
            }
            Console.WriteLine("----------");

            var gallery = await client.GetSubmissionsAsync(username, FAFolder.gallery);
            foreach (var s in gallery) {
                Console.WriteLine(s.title);
            }
        }
    }
}
