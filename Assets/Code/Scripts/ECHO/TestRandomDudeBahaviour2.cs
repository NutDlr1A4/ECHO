using UnityEngine;

public class TestRandomDudeBahaviour2 : MonoBehaviour, IHackable
{
    bool state = true;

    public void OnHack() {
        state = !state;

        if (state) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            gameObject.transform.Rotate(new Vector3(0, 0, 45));
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().color = Color.lightBlue;
            gameObject.transform.Rotate(new Vector3(0, 0, 45));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
