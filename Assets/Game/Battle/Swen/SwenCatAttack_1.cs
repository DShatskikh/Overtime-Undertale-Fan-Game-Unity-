using System.Collections;
using UnityEngine;

public sealed class SwenCatAttack_1 : MonoBehaviour
{
    public IEnumerator Start()
    {
        CreateShell(0, 0.7f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, -0.25f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, 0f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(1, 0.6f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(1, -0.15f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, -0.75f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, -0.8f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, -0.25f);
        yield return new WaitForSeconds(1.0f);
        CreateShell(0, 0.75f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, 0.8f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, 0.6f);
        yield return new WaitForSeconds(0.5f);
        CreateShell(0, 0.8f);
        yield return new WaitForSeconds(0.5f);
    }
    
    private void CreateShell(int indexType, float positionX)
    {
        var prefab = indexType switch
        {
            0 => Resources.Load<CryingCatShell_1>("Shells/Crying Cat Shell 1"),
            1 => Resources.Load<CryingCatShell_1>("Shells/Crying Cat Shell 2")
        };
        
        var shell = Instantiate(prefab,  transform.position + new Vector3(positionX, 0.64f), 
            Quaternion.identity, transform);
    }
}