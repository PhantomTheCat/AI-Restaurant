using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Customization
{
    /// <summary>
    /// Represents the type of environmental surface in a given context.
    /// </summary>
    public enum EnvironmentalType
    {
        Wall = 0,
        Ceiling = 1,
        Floor = 2,
    }


    /// <summary>
    /// Class applied to each GameObject that is customizable, and sets the material for that GameObject.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class MaterialSetter : MonoBehaviour
    {
        //Properties
        [SerializeField] public EnvironmentalType environmentalType;

        //Methods
        public void SetMaterial(Material material)
        {
            //Checking if the material is null or not
            if (material == null)
            {
                Debug.LogError("MaterialSetter: Attempted to set a null material.");
                return;
            }

            //Getting the Renderer component and setting the material
            var renderer = GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogError("MaterialSetter: No Renderer found on the GameObject.");
                return;
            }
            renderer.material = material;
        }
    }
}