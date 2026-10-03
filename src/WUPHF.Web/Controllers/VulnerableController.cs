using Microsoft.AspNetCore.Mvc;

namespace WUPHF.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  public class VulnerableController : ControllerBase
 {
        [HttpPost("xss-test")]
    public IActionResult TestXss([FromForm] string userInput, [FromForm] string userName)
        {
 // INTENTIONAL VULNERABILITY: Direct reflection of user input without encoding
    var htmlResponse = $@"
<!DOCTYPE html>
<html>
<head>
    <title>XSS Vulnerability Test Result</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; }}
        .vulnerable {{ background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 20px; margin: 20px 0; }}
        .warning {{ color: #d63384; font-weight: bold; }}
    </style>
</head>
<body>
    <h1>?? XSS Vulnerability Test Result</h1>
    <div class='warning'>WARNING: This page intentionally contains XSS vulnerabilities for educational purposes!</div>
    
    <div class='vulnerable'>
  <h3>Reflected User Input (VULNERABLE):</h3>
     <p><strong>Name:</strong> {userName}</p>
        <p><strong>Comment:</strong> {userInput}</p>
        <p>User {userName} says: {userInput}</p>
    </div>
    
    <script>
        console.log('Page loaded - XSS test complete');
  // Any injected scripts will execute here
    </script>
</body>
</html>";

       return Content(htmlResponse, "text/html");
        }

    [HttpGet("xss-test")]
 public IActionResult TestXssGet([FromQuery] string userInput = "", [FromQuery] string userName = "")
        {
        if (string.IsNullOrEmpty(userInput) && string.IsNullOrEmpty(userName))
            {
     var formHtml = @"
<!DOCTYPE html>
<html>
<head>
    <title>XSS Test Form</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .form-group { margin: 15px 0; }
    label { display: block; margin-bottom: 5px; }
        input, textarea { width: 100%; padding: 8px; }
        button { background-color: #dc3545; color: white; padding: 10px 20px; border: none; cursor: pointer; }
    </style>
</head>
<body>
 <h1>?? XSS Vulnerability Test Form</h1>
    <div style='background-color: #fff3cd; padding: 15px; margin-bottom: 20px;'>
        <strong>?? WARNING:</strong> This form is intentionally vulnerable to XSS attacks for educational purposes!
    </div>
    
    <form method='post' action='/api/vulnerable/xss-test'>
      <div class='form-group'>
            <label for='userName'>Your Name:</label>
     <input type='text' id='userName' name='userName' placeholder='Try: <img src=x onerror=alert(""XSS!"")>'>
      </div>
        
        <div class='form-group'>
            <label for='userInput'>Comment:</label>
      <textarea id='userInput' name='userInput' rows='4' placeholder='Try: <script>alert(""XSS Attack!"");</script>'></textarea>
        </div>
        
        <button type='submit'>Submit (Vulnerable)</button>
    </form>

    <hr style='margin: 30px 0;'>
    <h3>Test Payloads:</h3>
    <ul>
        <li><code>&lt;script&gt;alert('XSS');&lt;/script&gt;</code></li>
        <li><code>&lt;img src=x onerror=alert('XSS')&gt;</code></li>
        <li><code>&lt;svg onload=alert('XSS')&gt;</code></li>
        <li><code>&lt;iframe src=javascript:alert('XSS')&gt;&lt;/iframe&gt;</code></li>
    </ul>
</body>
</html>";
          return Content(formHtml, "text/html");
       }

     // Reflect the input back (VULNERABLE)
            return TestXss(userInput, userName);
        }

        [HttpPost("sql-injection-test")]
        public IActionResult TestSqlInjection([FromForm] string searchQuery)
  {
            // INTENTIONAL VULNERABILITY: Simulate SQL injection
         var simulatedQuery = $"SELECT * FROM Users WHERE name = '{searchQuery}'";
            
 var response = new
          {
        query = simulatedQuery,
           input = searchQuery,
         vulnerable = searchQuery.Contains(";") || searchQuery.Contains("--") || 
                searchQuery.ToLower().Contains("drop") || searchQuery.ToLower().Contains("union"),
           message = searchQuery.Contains(";") || searchQuery.Contains("--") ? 
         "?? SQL Injection detected! This could compromise the database!" :
         "Query executed (simulated)"
   };

  return Ok(response);
        }
    }
}