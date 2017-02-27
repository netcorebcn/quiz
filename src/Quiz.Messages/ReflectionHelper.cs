
using System.Reflection;

namespace Quiz.Messages
{
	public class ReflectionHelper {
		public static Assembly MessagesAssembly = typeof(ReflectionHelper).GetTypeInfo().Assembly; 
	}
}