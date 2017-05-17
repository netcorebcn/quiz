
using System.Reflection;

namespace Quiz.Domain
{
	public class ReflectionHelper {
		public static Assembly DomainAssembly = typeof(ReflectionHelper).GetTypeInfo().Assembly; 
	}
}