# 🔑 Password Generator CLI

A simple command-line application for generating strong passwords, verifying their entropy, and storing them securely in a file.  
Built with **.NET** and **System.CommandLine**.

---

## 🚀 Features
- Generate secure passwords with:
  - Custom length (4–25)
  - Uppercase letters
  - Digits
  - Special characters
- Copy password to clipboard
- Generate multiple passwords at once
- Calculate password entropy
- Save passwords to a file with labels

---

## ⚡ Usage
- Generate Passwords
  ```bash
  dotnet run -- password generate [options]
-Options
  - `--length, -l -> Password length(4 - 25, default: 20)`

  - ### Generate Passwords

`dotnet run -- password generate [options]`

**Options:**

- `--length, -l <number>` → Password length (default: 20, max: 25)
    
- `--uppercase, -u` → Include uppercase letters
    
- `--digit, -d` → Include digits
    
- `--special, -s` → Include special characters
    
- `--count, -c <number>` → Number of passwords to generate (default: 1)
    
- `--copy, -cp` → Copy the last generated password to clipboard
    

**Example:**

`dotnet run -- password generate -l 16 -u -d -s -c 3 -cp`

---

### Calculate Password Entropy

`dotnet run -- password entropy <password>`

**Example:**

`dotnet run -- password entropy My$ecret123`

---

### Save Password to File

`dotnet run -- password write [options] <password> <label>`

**Options:**

- `--file, -f <path>` → File path to save password (default: Desktop/ps.txt)
    

**Example:**

`dotnet run -- password write -f mypasswords.txt SuperStrongP@ssw0rd GitHub`

---

## 📋 Example Output

`K8a!qT5lP9s | Approximate password entropy: 72.54 | Password has been successfully written to the file.`
