using System.Reflection;
using System.Reflection.Emit;
using NetArchTest.Rules;


namespace Livraria.Tests.FitnessFunctions;


public class ArchTestsFixture
{
    public Types DomainAssembly { get; init; } = Types.InAssembly(Assembly.Load("Livraria.Domain"));
    public  Types ServiceAssembly { get; init; } = Types.InAssembly(Assembly.Load("Livraria.Services"));
    public Types InfrastructureAssembly { get; init; } = Types.InAssembly(Assembly.Load("Livraria.Infrastructure"));
    public Types ApiHostAssembly { get; init; } = Types.InAssembly(Assembly.Load("Livraria.ApiHost"));
    
    
    
    
    public bool IsMethodCalled(byte[] ilBytes, MethodInfo methodToFind)
    {
        // Get the method token for the base method
        int methodToken = methodToFind.MetadataToken | (methodToFind.Module.MetadataToken << 24); // Combine with module token

        // Iterate through the IL bytes
        for (int i = 0; i < ilBytes.Length; i++)
        {
            // Check for the OpCodes.Call opcode (0x28) or OpCodes.Callvirt opcode (0x6F)
            if (ilBytes[i] == OpCodes.Call.Value || ilBytes[i] == OpCodes.Callvirt.Value)
            {
                // Read the next 4 bytes as an integer (method metadata token)
                int operand = BitConverter.ToInt32(ilBytes, i + 1);

                // Adjust the operand check to match the method token
                if (operand == methodToken)
                {
                    return true; // Found a call to the target method
                }
            }
            else if (ilBytes[i] == OpCodes.Call.Value)
            {
                int token = BitConverter.ToInt32(ilBytes, i + 1);
                if (token == methodToFind.MetadataToken)
                {
                    return true; // Call found
                }
            }
        }

        return false; // Target method call not found
    }
}