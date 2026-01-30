# WUPHF Templates Directory

This directory contains message templates for the WUPHF platform.

## Available Templates

- `birthday.txt` - Birthday celebration template
- `emergency.txt` - Emergency notification template

## ⚠️ SECURITY WARNING ⚠️

The `/api/wuphf/template` endpoint that reads these templates contains an **intentional path traversal vulnerability** for security testing and educational purposes.

### Vulnerability Details

**Type:** Path Traversal (CWE-22)
**Severity:** Critical
**Affected Endpoint:** `GET /api/wuphf/template?path={path}`

### The Problem

The endpoint accepts a `path` query parameter and directly passes it to `File.ReadAllText()` without any validation:

```csharp
string templateContent = System.IO.File.ReadAllText(path);
```

### Attack Examples

An attacker can read ANY file on the server by manipulating the path parameter:

```
# Read system files on Linux
GET /api/wuphf/template?path=/etc/passwd

# Read sensitive configuration files
GET /api/wuphf/template?path=/home/runner/work/monorepo/monorepo/appsettings.json

# Read Windows system files
GET /api/wuphf/template?path=C:\Windows\System32\drivers\etc\hosts
```

### Proper Fix

To fix this vulnerability, the code should:

1. **Validate the path** - Only allow reading from a specific templates directory
2. **Sanitize input** - Remove path traversal sequences like `..` 
3. **Use Path.GetFullPath()** - Resolve the full path and verify it's within allowed directory
4. **Whitelist filenames** - Only allow specific template names

Example of secure implementation:

```csharp
[HttpGet("template")]
public ActionResult<object> GetTemplate([FromQuery] string templateName)
{
    // Define allowed templates directory
    string templatesDir = Path.Combine(Directory.GetCurrentDirectory(), "templates");
    
    // Sanitize the filename (remove path separators and traversal)
    string safeFileName = Path.GetFileName(templateName);
    
    // Build full path
    string fullPath = Path.GetFullPath(Path.Combine(templatesDir, safeFileName));
    
    // Verify the path is within the templates directory
    if (!fullPath.StartsWith(templatesDir))
    {
        return BadRequest("Invalid template name");
    }
    
    // Now it's safe to read
    string content = File.ReadAllText(fullPath);
    return Ok(new { Content = content });
}
```

## Purpose

This vulnerability is intentionally included to:
- Demonstrate common security flaws in file handling
- Test security scanning tools (CodeQL, SAST)
- Educate developers about path traversal attacks
- Provide a controlled environment for security testing

**DO NOT deploy this code to production environments!**

---

*"This is going to revolutionize the template industry!"* - Ryan Howard
