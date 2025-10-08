using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDIExample.Services;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class LogExecutionAttribute : Attribute
{
    public string? Message { get; set; }

    public LogExecutionAttribute() { }

    public LogExecutionAttribute(string message)
    {
        Message = message;
    }
}