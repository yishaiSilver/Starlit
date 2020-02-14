using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoslynCSharp;

public class ShipScript : MonoBehaviour {

    public string filename;

    private ScriptDomain domain = null;
    private ScriptType type = null;
    private ScriptProxy proxy = null;

    public bool shouldExecute = false;

    public Auto loaded;

    public void FixedUpdate()
    {
        if (loaded != null && shouldExecute)
        {
            loaded.main();
        }
        else if (proxy != null && shouldExecute)
        {
            proxy.SafeCall("main");
        }
    }
   
    public void ExecuteCode()
    {
        if (loaded != null && shouldExecute)
        {
            loaded.main();
        }
        else if (proxy != null && shouldExecute)
        {
            proxy.SafeCall("Main");
        }
    }

    public bool LoadScript(Ship ship, StarSystem starSystem, string filename)
    {
        if (domain != null)
        {
            domain.Dispose();
        }

        domain = ScriptDomain.CreateDomain("ShipDomain", true);

        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(UnityEngine.Object).Assembly);
        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(Ship).Assembly);
        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(StarSystem).Assembly);
        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(LandableObject).Assembly);

        type = domain.CompileAndLoadMainFile(filename);

        if (proxy != null && !proxy.IsDisposed)
        {
            Debug.Log(proxy.IsDisposed);
            proxy.Dispose();
        }

        Debug.Log(gameObject.name);
        Debug.Log(type.Name);

        proxy = type.CreateInstance(gameObject);
        Debug.Log(gameObject.name);

        if (type != null)
        {
            proxy.Fields["ship"] = ship;
            proxy.Fields["starSystem"] = starSystem;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UpdateScript(Ship ship, StarSystem starSystem, string code)
    {
        if(domain != null)
        {
            domain.Dispose();
        }

        domain = ScriptDomain.CreateDomain("ShipDomain", true);

        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(UnityEngine.Object).Assembly);
        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(Ship).Assembly);
        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(StarSystem).Assembly);
        //domain.RoslynCompilerService.ReferenceAssemblies.Add(typeof(LandableObject).Assembly);

        type = domain.CompileAndLoadMainSource(@code);

        if (proxy != null && !proxy.IsDisposed)
        {
            Debug.Log(proxy.IsDisposed);
            proxy.Dispose();
        }

        Debug.Log(gameObject.name);
        Debug.Log(type.Name);

        proxy = type.CreateInstance(gameObject);
        Debug.Log(gameObject.name);

        if(type != null)
        {
            proxy.Fields["ship"] = ship;
            proxy.Fields["starSystem"] = starSystem;
            return true;
        }
        else
        {
            return false;
        }
    }
}
