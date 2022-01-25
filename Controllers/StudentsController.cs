using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace cwiczenia3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {

        private readonly ILogger<StudentsController> _logger;
        private static string path = @"C:\dane.csv";
        TextFieldParser csvParser = new TextFieldParser(path);

        public StudentsController(ILogger<StudentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Students> Get()
        {
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = false;
            List<Students> studentsList = new List<Students>();
            while (!csvParser.EndOfData)
            {
                Students student = new Students();
                string[] fields = csvParser.ReadFields();
                student.name = fields[0];
                student.surname = fields[1];
                student.id = fields[4];
                student.birthdate = fields[5];
                student.studies = fields[2];
                student.mode = fields[3];
                student.email = fields[6];
                student.fathersName = fields[8];
                student.mothersName = fields[7];
                studentsList.Add(student);
            }
            return studentsList;
        }

        [HttpGet("{id}")]
        public List<Students> Get(String id)
        {
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = false;
            List<Students> studentsList = new List<Students>();
            while (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields();
                if (id.Remove(0, 1) == fields[4])
                {
                    Students student = new Students();
                    student.name = fields[0];
                    student.surname = fields[1];
                    student.id = fields[4];
                    student.birthdate = fields[5];
                    student.studies = fields[2];
                    student.mode = fields[3];
                    student.email = fields[6];
                    student.fathersName = fields[8];
                    student.mothersName = fields[7];
                    studentsList.Add(student);
                }
            }
            return studentsList;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent([FromBody] JsonElement body)
        {
            string id = body.GetProperty("id").ToString();
            List<string> CSVData = new List<string>();
            while (!csvParser.EndOfData)
            {
                Students student = new Students();
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                string[] fields = csvParser.ReadFields();
                if (fields[4] == body.GetProperty("id").ToString())
                {
                    student.name = body.GetProperty("name").ToString();
                    student.surname = body.GetProperty("surname").ToString();
                    student.id = body.GetProperty("id").ToString();
                    student.birthdate = body.GetProperty("birthdate").ToString();
                    student.studies = body.GetProperty("studies").ToString();
                    student.mode = body.GetProperty("mode").ToString();
                    student.email = body.GetProperty("email").ToString();
                    student.fathersName = body.GetProperty("fathersName").ToString();
                    student.mothersName = body.GetProperty("mothersName").ToString();
                } else
                {
                    student.name = fields[0];
                    student.surname = fields[1];
                    student.id = fields[4];
                    student.birthdate = fields[5];
                    student.studies = fields[2];
                    student.mode = fields[3];
                    student.email = fields[6];
                    student.fathersName = fields[8];
                    student.mothersName = fields[7];
                }
                CSVData.Add(student.CSV());
            }
            TextWriter textWriter = new StreamWriter(path);
            foreach (string lineToAdd in CSVData)
            {
                textWriter.WriteLine(lineToAdd);
            }
            textWriter.Close();
            return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete(String id)
        {
            List<string> linesToRemove = new List<string>();
            while (!csvParser.EndOfData)
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                string[] fields = csvParser.ReadFields();
                if (id == fields[4])
                {
                    string lineToRemove = fields[0] + "," 
                        + fields[1] + "," 
                        + fields[2] + "," 
                        + fields[3] + "," 
                        + fields[4] + "," 
                        + fields[5] + "," 
                        + fields[6] + "," 
                        + fields[7] + "," 
                        + fields[8];
                    linesToRemove.Add(lineToRemove);
                }
            }
            List<String> CSVdata = new List<String>();
            string line;
            using (StreamReader sr = new StreamReader(path))
            while ((line = sr.ReadLine()) != null)
            {
                CSVdata.Add(line);
            }
            Console.WriteLine("Delete id=" + id);
            Console.WriteLine("lines to remove:");
            foreach (string l in linesToRemove)
            {
                Console.WriteLine(l);
                CSVdata.Remove(l);
            }
            Console.WriteLine("\n\n\n\n\n");
            TextWriter textWriter = new StreamWriter(path);
            foreach (String s in CSVdata)
            {
                textWriter.WriteLine(s);
            }
            textWriter.Close();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] JsonElement body)
        {
            int hiId = 0;
            while (!csvParser.EndOfData)
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                string[] fields = csvParser.ReadFields();
                if (hiId < int.Parse(fields[4]))
                {
                    hiId = int.Parse(fields[4]);
                }
            }
            string name = body.GetProperty("name").ToString();
            string surname = body.GetProperty("surname").ToString();
            string id = (hiId + 1).ToString();
            string birthdate = body.GetProperty("birthdate").ToString();
            string studies = body.GetProperty("studies").ToString();
            string mode = body.GetProperty("mode").ToString();
            string email = body.GetProperty("email").ToString();
            string fathersName = body.GetProperty("fathersName").ToString();
            string mothersName = body.GetProperty("mothersName").ToString();
            string lineToAdd =
                name + "," +
                surname + "," +
                studies + "," +
                mode + "," +
                id + "," +
                birthdate + "," +
                email + "," +
                mothersName + "," +
                fathersName;
            TextWriter textWriter = new StreamWriter(path, true);
            textWriter.WriteLine(lineToAdd);
            textWriter.Close();
            Console.WriteLine("Added: " + lineToAdd);
            return Ok();
        }
    }
}
