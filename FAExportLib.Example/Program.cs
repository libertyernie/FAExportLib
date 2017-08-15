using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAExportLib.Example {
    class Program {
        static void Main(string[] args) {
            PostJournalAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync() {
            // Create a client object
            //FAClient client = new FAUserClient(a: "11111111-1111-1111-1111-111111111111", b: "22222222-2222-2222-2222-222222222222");
            FAClient client = new FAClient();

            Console.Write("Enter a FurAffinity username (default: lizard-socks): ");
            string username = Console.ReadLine();
            if (string.IsNullOrEmpty(username)) username = "lizard-socks";

            // Check if we're logged in
            if (client is FAUserClient userClient) {
                Console.Write("Logged in: ");
                Console.WriteLine(await userClient.WhoamiAsync() ?? "nobody");
            }
            Console.WriteLine("----------");

            // Get information about the entered user
            var user = await client.GetUserAsync(username);
            Console.WriteLine(user.name);
            if (user.contact_information != null) {
                foreach (var ci in user.contact_information) {
                    Console.WriteLine($"{ci.title}: {ci.name}");
                }
            }
            Console.WriteLine("----------");

            // Get the details of some recent submissions
            var submissions = await client.GetSubmissionsAsync(username, FAFolder.gallery);
            foreach (var s in submissions.Take(3)) {
                var submission = await client.GetSubmissionAsync(s.id);
                Console.WriteLine(submission.title);
                Console.WriteLine("    " + submission.category);
                Console.WriteLine("    " + submission.species);
                Console.WriteLine("    " + submission.gender);
                Console.WriteLine("    " + submission.download);
                Console.WriteLine("    " + submission.rating);
                Console.WriteLine(submission.description);
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                Console.WriteLine("----------");
            }

            // Get up to 3 pages of submission data
            int page = 1;
            while (true) {
                if (page > 3) break;
                Console.WriteLine($"--PAGE {page}--");
                var gallery = await client.GetSubmissionsAsync(username, FAFolder.gallery, page++);
                if (gallery.Count() == 0) {
                    Console.WriteLine("No more submissions.");
                    break;
                }
                foreach (var s in gallery) {
                    Console.WriteLine(s.title);
                }

                Console.Write("Press enter to continue...");
                Console.ReadLine();
                Console.WriteLine("----------");
            }
        }

        static async Task PostJournalAsync() {
            FAUserClient client = new FAUserClient(a: "11111111-1111-1111-1111-111111111111", b: "22222222-2222-2222-2222-222222222222");

            Console.Write("Enter title: ");
            string title = Console.ReadLine();
            Console.WriteLine("Enter body (multiple lines allowed, use END to end)");
            StringBuilder body = new StringBuilder();
            while (true) {
                string line = Console.ReadLine();
                if (line == null || line == "END") break;
                body.AppendLine(line);
            }

            string url = await client.PostJournalAsync(title, body.ToString());

            Process.Start(url);
        }
    }
}
