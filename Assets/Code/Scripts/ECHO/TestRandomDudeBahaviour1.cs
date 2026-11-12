using UnityEngine;

public class TestRandomDudeBahaviour1 : MonoBehaviour, IHackable
{
    bool state = true;

    public void OnHack() {
        state = !state;

        if (state) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().color = Color.lightBlue;
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
