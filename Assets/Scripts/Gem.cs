using UnityEngine;

public class Gem : MonoBehaviour
{

    ///---------funcs we need;---- "setPosition" so everytime we move the gem we update its position---- 
    ///----------we also need a function that checks which way we moved the gem to; up down right left

    /*
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() ///----anything in start happens the second we press play
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */

    //public Board board; //to store which board owns the gem-it gets filled when the board spawns the gem and it lets the gem call back into the Board's logic
    public int row , col ; //which slot the gem is in
    public void OnMouseDown()
    {
        Debug.Log("clicked gem at row" + row + "col" + col);
    }






} //public class Gem : MonoBehaviour
