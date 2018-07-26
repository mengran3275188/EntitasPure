using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas.CodeGeneration
{
    public enum DiagnosisSeverity
    {
        Healthy,
        Hint,
        Warning,
        Error
    }
    public class Diagnosis
    {
        public readonly string symptoms;
        public readonly string treatment;
        public readonly DiagnosisSeverity serverity;
        public static Diagnosis Healthy
        {
            get
            {
                return new Diagnosis(null, null, DiagnosisSeverity.Healthy);
            }
        }
        public Diagnosis(string symptoms, string treatment, DiagnosisSeverity severity)
        {
            this.symptoms = symptoms;
            this.treatment = treatment;
            this.serverity = serverity;
        }
    }
}
