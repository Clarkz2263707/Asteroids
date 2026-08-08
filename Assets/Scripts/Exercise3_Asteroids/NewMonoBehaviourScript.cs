using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject largeAsteroid;
    [SerializeField] private GameObject mediumAsteroid;
    [SerializeField] private GameObject smallAsteroid;

    private GameObject AsteroidSize(Asteroid.AsteroidSize Size)
    {
        switch (Size)
        {
            case Asteroid.AsteroidSize.Large:
                return largeAsteroid;
                break;
            case Asteroid.AsteroidSize.Medium:
                return mediumAsteroid;
                break;
            case Asteroid.AsteroidSize.Small:
                return smallAsteroid;
                break;
        }
        return null;
    }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
