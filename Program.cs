using System;
using System.Collections.Generic;
using MySqlConnector;

namespace ConsoleDatabase {
    class Program {
        static void Main() {
            var quizContext = new QuizContext("server=localhost;user=root;password=;database=quiz");
            var questionList = quizContext.GetQuestions();

            foreach(var question in questionList) {
                Game.Showcase(question);
                string answer = Console.ReadLine();

                if(answer != null) {
                    Console.WriteLine(answer[0] == question.Answer ? "Poprawna odpowiedz!" : "Zla odpowiedz!");
                }
            }
            
            Console.WriteLine("Koniec gry!");
        }
    }

    public static class Game {
        public static void Showcase(Question question) {
            Console.WriteLine($"Question nr {question.Id}");
            Console.WriteLine(question.Description);
        }
    }

    public class Question {
        public int Id { get; set; }
        public string Description { get; set; }
        public char Answer { get; set; }
        public int Points { get; set; }
    }

    public class QuizContext {
        private string connectionString;

        public QuizContext(string connectionString) {
            this.connectionString = connectionString;
        }

        private MySqlConnection GetConnection() {
            return new MySqlConnection(connectionString);
        }

        public List<Question> GetQuestions() {
            List<Question> questions = new List<Question>();

            using(MySqlConnection connection = GetConnection()) {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM pytania", connection);

                using(MySqlDataReader reader = command.ExecuteReader()) {
                    while(reader.Read()) {
                        questions.Add(
                            new Question {
                                Id = reader.GetInt32("id"),
                                Description = reader.GetString("pytanie"),
                                Answer = reader.GetChar("odpowiedz"),
                                Points = reader.GetInt32("punkty"),
                            });
                    }
                }
            }

            return questions;
        }
    }
}
