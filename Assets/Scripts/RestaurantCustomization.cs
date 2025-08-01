using UnityEngine;

namespace Customization
{
    /// <summary>
    /// Holds the customization options for a restaurant, including name and materials for walls, floors, and ceilings.
    /// </summary>
    public class RestaurantCustomization : MonoBehaviour
    {
        // Properties
        [Header("Customization Options")]
        [SerializeField] private string restaurantName = "My Restaurant";
        /// <summary>
        /// Potential wall materials to use
        /// </summary>
        [SerializeField] private Material[] wallMaterials;
        /// <summary>
        /// Potential floor materials to use
        /// </summary>
        [SerializeField] private Material[] floorMaterials;
        /// <summary>
        /// Potential ceiling materials to use
        /// </summary>
        [SerializeField] private Material[] ceilingMaterials;


        // Methods
        private void Start()
        {
            //Making sure each material array is not null or empty
            CheckMaterialArrays();
        }

        public void SetRestaurantName(string name)
        {
            restaurantName = name;
        }

        /// <summary>
        /// Updates the material for the specified environmental type based on the provided index.
        /// </summary>
        /// <param name="environmentalType"></param>
        /// <param name="materialIndex"></param>
        public void ChangeBaseMaterial(EnvironmentalType environmentalType, int materialIndex)
        {
            switch (environmentalType)
            {
                case EnvironmentalType.Wall:
                    if (materialIndex >= 0 && materialIndex < wallMaterials.Length)
                    {
                        SetMaterials(wallMaterials[materialIndex], environmentalType);
                    }
                    else
                    {
                        Debug.LogError("Invalid wall material index.");
                    }
                    break;
                case EnvironmentalType.Floor:
                    if (materialIndex >= 0 && materialIndex < floorMaterials.Length)
                    {
                        SetMaterials(floorMaterials[materialIndex], environmentalType);
                    }
                    else
                    {
                        Debug.LogError("Invalid floor material index.");
                    }
                    break;
                case EnvironmentalType.Ceiling:
                    if (materialIndex >= 0 && materialIndex < ceilingMaterials.Length)
                    {
                        SetMaterials(ceilingMaterials[materialIndex], environmentalType);
                    }
                    else
                    {
                        Debug.LogError("Invalid ceiling material index.");
                    }
                    break;
                default:
                    Debug.LogError("Unknown environmental type in the RestaurantCustomization script.");
                    break;
            }
        }

        /// <summary>
        /// Sets the material for all GameObjects with a MaterialSetter component that match the specified environmental type.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="environmentalType"></param>
        private void SetMaterials(Material material, EnvironmentalType environmentalType)
        {
            // Find all MaterialSetter components in the scene
            MaterialSetter[] materialSetters = FindObjectsByType<MaterialSetter>(FindObjectsSortMode.None);

            // Loop through each MaterialSetter and set the material based on the environmental type
            foreach (MaterialSetter setter in materialSetters)
            {
                if (setter.environmentalType == environmentalType)
                {
                    setter.SetMaterial(material);
                }
            }
        }

        private void CheckMaterialArrays()
        {
            if (wallMaterials == null || wallMaterials.Length == 0)
            {
                Debug.LogError("Wall materials are not set or empty in RestaurantCustomization.");
            }
            if (floorMaterials == null || floorMaterials.Length == 0)
            {
                Debug.LogError("Floor materials are not set or empty in RestaurantCustomization.");
            }
            if (ceilingMaterials == null || ceilingMaterials.Length == 0)
            {
                Debug.LogError("Ceiling materials are not set or empty in RestaurantCustomization.");
            }
        }
    }
}