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
            //FAClient client = new FAClient(a: "11111111-1111-1111-1111-111111111111", b: "11111111-1111-1111-1111-111111111111");
            FAClient client = new FAClient();

            Console.Write("Enter a FurAffinity username (default: lizard-socks): ");
            string username = Console.ReadLine();
            if (string.IsNullOrEmpty(username)) username = "lizard-socks";

            Console.WriteLine(await client.WhoamiAsync() ?? "No user logged in");
            Console.WriteLine("----------");

            var user = await client.GetUserAsync(username);
            Console.WriteLine(user.name);
            if (user.contact_information != null) {
                foreach (var ci in user.contact_information) {
                    Console.WriteLine($"{ci.title}: {ci.name}");
                }
            }
            Console.WriteLine("----------");

            int page = 1;
            int i = 0;
            while (true) {
                if (page > 3) break;
                Console.WriteLine($"--PAGE {page}--");
                var gallery = await client.GetSubmissionsAsync(username, FAFolder.gallery, page++);
                if (gallery.Count() == 0) break;
                foreach (var s in gallery) {
                    Console.WriteLine(s.title);
                    if (++i < 3) {
                        var submission = await client.GetSubmissionAsync(s.id);
                        Console.WriteLine("    " + submission.category);
                        Console.WriteLine("    " + submission.species);
                        Console.WriteLine("    " + submission.gender);
                        Console.WriteLine("    " + submission.download);
                        Console.WriteLine("    " + submission.rating);
                        Console.WriteLine("----------");
                    }
                }

                Console.WriteLine("----------");
            }
        }
    }
}
