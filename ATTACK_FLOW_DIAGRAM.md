# Path Traversal Attack Flow Diagram

```
┌─────────────────────────────────────────────────────────────────────────┐
│                       WUPHF Path Traversal Attack                       │
└─────────────────────────────────────────────────────────────────────────┘

1. ATTACKER
   │
   │  HTTP Request
   │  GET /api/wuphf/template?path=/etc/passwd
   │
   ▼
2. WUPHF API CONTROLLER
   │
   │  WuphfController.GetTemplate(string path)
   │  {
   │    // ❌ NO VALIDATION!
   │    string content = File.ReadAllText(path);
   │  }
   │
   ▼
3. FILESYSTEM ACCESS
   │
   │  File.ReadAllText("/etc/passwd")
   │
   │  ┌──────────────────────────────────────┐
   │  │  File System                         │
   │  │                                       │
   │  │  ✅ /etc/passwd (READ)               │
   │  │  ✅ /home/user/.ssh/id_rsa (READ)    │
   │  │  ✅ appsettings.json (READ)          │
   │  │  ✅ Any file with read permission!   │
   │  └──────────────────────────────────────┘
   │
   ▼
4. RESPONSE
   │
   │  HTTP 200 OK
   │  {
   │    "templatePath": "/etc/passwd",
   │    "content": "root:x:0:0:root:/root:/bin/bash\n...",
   │    "message": "Template loaded! Ready to WUPHF!"
   │  }
   │
   ▼
5. ATTACKER
   │
   │  ✅ SUCCESS! Sensitive file contents leaked
   └──


═══════════════════════════════════════════════════════════════════════════

SECURE IMPLEMENTATION FLOW:

1. ATTACKER
   │
   │  HTTP Request
   │  GET /api/wuphf/template?path=../../etc/passwd
   │
   ▼
2. WUPHF API CONTROLLER (SECURED)
   │
   │  WuphfController.GetTemplate(string templateName)
   │  {
   │    ✅ Validation 1: sanitize input
   │    string safe = Path.GetFileName(templateName);
   │
   │    ✅ Validation 2: build safe path
   │    string full = Path.Combine(templatesDir, safe);
   │
   │    ✅ Validation 3: verify boundaries
   │    if (!full.StartsWith(templatesDir))
   │      return BadRequest();
   │
   │    ✅ Validation 4: check existence
   │    if (!File.Exists(full))
   │      return NotFound();
   │
   │    string content = File.ReadAllText(full);
   │  }
   │
   ▼
3. FILESYSTEM ACCESS (RESTRICTED)
   │
   │  ┌──────────────────────────────────────┐
   │  │  File System                         │
   │  │                                       │
   │  │  ✅ /templates/birthday.txt (OK)     │
   │  │  ✅ /templates/emergency.txt (OK)    │
   │  │  ❌ /etc/passwd (BLOCKED)            │
   │  │  ❌ ../appsettings.json (BLOCKED)    │
   │  └──────────────────────────────────────┘
   │
   ▼
4. RESPONSE
   │
   │  HTTP 400 Bad Request
   │  {
   │    "error": "Invalid template name"
   │  }
   │
   ▼
5. ATTACKER
   │
   │  ❌ BLOCKED! Attack prevented
   └──


═══════════════════════════════════════════════════════════════════════════

KEY DIFFERENCES:

VULNERABLE CODE:
❌ No input validation
❌ Direct file access with user input
❌ No path boundary checks
❌ No allowlist of permitted files/directories

SECURE CODE:
✅ Input sanitization (Path.GetFileName)
✅ Path construction using safe APIs (Path.Combine)
✅ Boundary verification (StartsWith check)
✅ Restricted to specific directory
✅ File existence validation
✅ Structured logging to prevent log forging

═══════════════════════════════════════════════════════════════════════════

DEFENSE IN DEPTH:

Layer 1: Input Validation
  - Sanitize user input
  - Reject suspicious patterns (.. / \)

Layer 2: Path Resolution
  - Use Path.GetFileName() to strip directory info
  - Use Path.Combine() for safe path construction
  - Use Path.GetFullPath() to resolve to absolute path

Layer 3: Boundary Check
  - Verify resolved path is within allowed directory
  - Use string comparison with directory separator

Layer 4: File System Permissions
  - Run application with minimal required permissions
  - Restrict read access to necessary directories only

Layer 5: Monitoring & Logging
  - Log all file access attempts
  - Alert on suspicious patterns
  - Sanitize logged data to prevent log injection

═══════════════════════════════════════════════════════════════════════════
```

## Visual Representation

```
VULNERABLE ENDPOINT:

  User Input
      │
      │ path="/etc/passwd"
      │
      ▼
┌─────────────────┐
│  Controller     │ ❌ No validation
│  GetTemplate()  │
└────────┬────────┘
         │
         │ File.ReadAllText(path)
         │
         ▼
┌─────────────────┐
│  File System    │ 💥 Any file readable!
└─────────────────┘


SECURE ENDPOINT:

  User Input
      │
      │ templateName="../../etc/passwd"
      │
      ▼
┌─────────────────┐
│  Controller     │ ✅ Path.GetFileName()
│  GetTemplate()  │ ✅ Path.Combine()
└────────┬────────┘ ✅ Boundary check
         │
         │ Resolved: "/templates/passwd" (not in templatesDir)
         │
         ▼
┌─────────────────┐
│  Validation     │ ❌ Blocked!
│  Failed         │ → BadRequest()
└─────────────────┘
```

## Example Attack Payloads

| Payload | Target (Linux) | Target (Windows) |
|---------|---------------|------------------|
| `/etc/passwd` | System users | N/A |
| `../../../../etc/shadow` | Password hashes | N/A |
| `/home/user/.ssh/id_rsa` | SSH private key | N/A |
| `appsettings.json` | App config | App config |
| `../../../web.config` | Web config | Web config |
| `C:\Windows\System32\drivers\etc\hosts` | N/A | Hosts file |
| `.env` | Environment vars | Environment vars |

## References

- OWASP Top 10 2021: A01:2021 – Broken Access Control
- CWE-22: Improper Limitation of a Pathname to a Restricted Directory
- SANS Top 25: CWE-22 ranks #8 in most dangerous software weaknesses

---
*"This is going to revolutionize the... wait, why can you read my password file?" - Ryan Howard*
