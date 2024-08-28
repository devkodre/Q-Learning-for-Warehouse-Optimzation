using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Spawner : MonoBehaviour
{
    public GameObject cubePref;

    float i = 0;
    List<cube> cubes = new List<cube>();
    System.Random rnd = new System.Random();
    List<Vector3> spaces = new List<Vector3>();
    int [] SIZES = {1, 3, 5};
    bool safe = true;
    int rc = 0;
    GameObject clone;
    Vector3 size;
    Vector3 target;
    Vector3 [] ls = new Vector3[2];
    float S = 7f;
    List<Vector3> posList = new List<Vector3>();
    List<Vector3> sizeList = new List<Vector3>();

    // Update is called once per frame
    void Start()
    {
        //for (float x=-24.5f; x < 25; x+=1)
            //for (float y=0.5;y < 25; y+=1)
                //for(float z=-24.5f; z < 25; z+=1)
                    //spaces.Add(new Vector3(x,y,z));

        for (float x=0f; x < S; x+=1f)
            for (float y=0f;y < S; y+=1f)
                for(float z=0f; z < S; z+=1f)
                    spaces.Add(new Vector3(x,y,z));
        //printList();
        print(spaces.Count);

        try
        {
            // Create a StreamReader
            using (StreamReader reader = new StreamReader("/Users/devkodre/My project/Assets/cubes.txt"))
            {
                string line;
                // Read line by line
                while ((line = reader.ReadLine()) != null)
                {
                    //Store positions and sizes
                    string[] parts = line.Replace("[", "").Replace("]", "").Split(',');
                    if (parts.Length == 6) // Ensure correct format
                    {
                        float posX, posY, posZ, sizeX, sizeY, sizeZ;
                        if (float.TryParse(parts[0], out posX) &&
                            float.TryParse(parts[1], out posY) &&
                            float.TryParse(parts[2], out posZ) &&
                            float.TryParse(parts[3], out sizeX) &&
                            float.TryParse(parts[4], out sizeY) &&
                            float.TryParse(parts[5], out sizeZ))
                        {
                            Vector3 position = new Vector3(posX, posY, posZ);
                            Vector3 size = new Vector3(sizeX, sizeY, sizeZ);

                            // Add to lists
                            posList.Add(position);
                            sizeList.Add(size);
                        }
                    }
                }
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp.Message);
        }        
    }
    void Update()
    {
        safe = true;
        i = 0;
        rc = 0;
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            //size = new Vector3(SIZES[rnd.Next(3)], SIZES[rnd.Next(3)], SIZES[rnd.Next(3)]);
           
            //print(size);
            //ls = getBestPosition(cubes, size);
            //target = ls[0];
            //size = ls[1];
            target = new Vector3(-25,-25,-25);

            if (posList.Count > 0 && sizeList.Count > 0)
            {
                target =  posList[0];
                size = sizeList[0];

                // Remove the first element
                posList.RemoveAt(0);
                sizeList.RemoveAt(0);

                // Now firstPos and firstSize contain the popped values
                // Use them as needed
            }
            else
            {
                print("List Empty");
            }
        
            if (safe)
            {
                clone = Instantiate(cubePref, transform.position, Quaternion.identity);
                cube c = clone.GetComponent<cube>();
                c.setSize(size);
                c.target = target;


                
                // REPLACE this with find best point method
                updatePositions(c);
                cubes.Add(c);

                
            }
        
        }

    }
    void move(GameObject cube, Vector3 target)
    {
        cube.GetComponent<cube>().move(target);
    }

    public Vector3 [] getBestPosition(List<cube> cubes, Vector3 size)
    {
        //rc+=1;
        List<Vector3> valids = new List<Vector3>();
        List<Vector3> legals = new List<Vector3>();
        Vector3 [] v = new Vector3[2];
        valids = validSpaces(size);
        if (valids.Count == 0 || rc > 500)
        {
            print("No space available");
            v[0] = new Vector3(-25,-25,-25);
            v[1] = new Vector3(0,0,0);
            return v;
        }
    // Checks if its on a cube or on the floor (not floating in mid air)
        for (int a = 0; a < valids.Count;a+=1)
        {
            int bottom = (int)valids[a].y - (int)Math.Truncate(size.y/2);
            if (bottom == 0)
            {
                legals.Add(valids[a]);
                continue;
            }
            
            for (int j = 0; j < cubes.Count; j+=1)
            {

                cube c = cubes[j];
                if (isOver(valids[a],size,c))
                {
                    legals.Add(valids[a]);
                    break;
                }
                //int top = (int)Math.Truncate(c.getSize().y/2) + (int)c.getPos().y;
                //if (top == bottom)
                //{
                //    print("BOTTOM Pos: " + c.getPos() + " BOTTOM Size: " + c.getSize());
                    //print("TOP Pos: " + valids[a] + " Top Size: " + size);

                    //legals.Add(valids[a]);
                //}
            } 
        }
            //return {new Vector3(-25,-25,-25), new Vector3(0,0,0)};
        int i = rnd.Next(0, legals.Count);
        //print("v" + legals[i]);

/*        for (float x = (float)Math.Truncate(-size.x/2); x < size.x/2; x+=1)
        {
            if (spaces[i].x + x  < 0 || spaces[i].x + x > 5)
                valid = false;
        }
                
        for (float y = (float)Math.Truncate(-size.y/2); y < size.y/2; y+=1)
        {
            if (spaces[i].y + y < 0 || spaces[i].y + y > 5)
                valid = false;
        }

        for (float z = (float)Math.Truncate(-size.z/2); z < size.z/2; z+=1)
        {
            if (spaces[i].z + z < 0 || spaces[i].z + z > 5)
                valid = false;
        }

        for (float x = (float)Math.Truncate(-size.x/2); x <= size.x/2; x+=1)
            for (float y = (float)Math.Truncate(-size.y/2); y <= size.y/2; y+=1)
                for(float z = (float)Math.Truncate(-size.z/2); z <= size.z/2; z+=1)
                    {
                        
                        Vector3 a = spaces[i] + new Vector3(x,y,z);
                        if (!spaces.Contains(a))
                            return getBestPosition(cubes, new Vector3(SIZES[rnd.Next(3)], SIZES[rnd.Next(3)], SIZES[rnd.Next(3)]));
                    }
*/                        

        //if (isValidPosition(i,size) == false)
            //return getBestPosition(cubes, new Vector3(SIZES[rnd.Next(3)], SIZES[rnd.Next(3)], SIZES[rnd.Next(3)]));
        v[0] = legals[i];
        v[1] = size;
        return v;

        /*if (cubes.Count == 0)
        {
            print("Size: " + size);
            print("Target: " + new Vector3(0f,0.5f,0f));
            return new Vector3(0f,0.5f,0f);
        }
        else
        {
            cube prev = cubes[cubes.Count-1];
            Vector3 pos = prev.target;
            Vector3 pSize = prev.getSize();
            Vector3 newPos = new Vector3(pos.x+size.x/2+pSize.x/2 ,0.5f,0f);
            print("Size: " + size);
            print("Target: " + newPos);
            return newPos;

        }*/
    }

    public bool isValidPosition(int i, Vector3 size)
    {
        for (float x = (float)Math.Truncate(-size.x/2); x < size.x/2; x+=1)
        {
            if (spaces[i].x + x  < 0 || spaces[i].x + x > 5)
                return false;
        }
                
        for (float y = (float)Math.Truncate(-size.y/2); y < size.y/2; y+=1)
        {
            if (spaces[i].y + y < 0 || spaces[i].y + y > 5)
                return false;
        }

        for (float z = (float)Math.Truncate(-size.z/2); z < size.z/2; z+=1)
        {
            if (spaces[i].z + z < 0 || spaces[i].z + z > 5)
                return false;
        }

        for (float x = (float)Math.Truncate(-size.x/2); x <= size.x/2; x+=1)
            for (float y = (float)Math.Truncate(-size.y/2); y <= size.y/2; y+=1)
                for(float z = (float)Math.Truncate(-size.z/2); z <= size.z/2; z+=1)
                    {
                        
                        Vector3 a = spaces[i] + new Vector3(x,y,z);
                        if (!spaces.Contains(a))
                            return false;
                    }
        return true;
    }

    public List<Vector3> validSpaces(Vector3 size)
    {
        bool flag = false;
        List<Vector3> spcCopy = new List<Vector3>(); 


        for(int i = 0; i < spaces.Count ; i+=1)
        {
            flag = false;
            for (float x = (float)Math.Truncate(-size.x/2); x < size.x/2; x+=1)
            {
                if (spaces[i].x + x  < 0 || spaces[i].x + x > S)
                    flag = true;
                    break;
            }
            if (flag == true) continue;
                    
            for (float y = (float)Math.Truncate(-size.y/2); y < size.y/2; y+=1)
            {
                if (spaces[i].y + y < 0 || spaces[i].y + y > S)
                    flag = true;
                    break;
            }
            if (flag == true) continue;

            for (float z = (float)Math.Truncate(-size.z/2); z < size.z/2; z+=1)
            {
                if (spaces[i].z + z < 0 || spaces[i].z + z > S)
                    flag = true;
                    break;
            }
            if (flag == true) continue;

            for (float x = (float)Math.Truncate(-size.x/2); x <= size.x/2; x+=1)
            {
                for (float y = (float)Math.Truncate(-size.y/2); y <= size.y/2; y+=1)
                {
                    for(float z = (float)Math.Truncate(-size.z/2); z <= size.z/2; z+=1)
                        {
                            
                            Vector3 a = spaces[i] + new Vector3(x,y,z);
                            if (!spaces.Contains(a))
                            {
                                flag = true;
                                break;
                            }
                        }
                    if (flag) break;
                }
                if (flag) break;
            }
            if (flag) continue;

            spcCopy.Add(spaces[i]);
        }
        return spcCopy;
    }

    public bool isOver(Vector3 position, Vector3 size,cube c)
    {
        
        Vector3 space = c.getPos();
        Vector3 cSize = c.getSize();
        //First check if the position is within the x and z of the cube its gonna be on
        if ((space.x - cSize.x/2) < position.x && position.x < (space.x + cSize.x/2) && (space.z - cSize.z/2) < position.z && position.z < (space.z + cSize.z/2)) 
        {
            // Check if the top of the cube == the bottom of the position and size
            //print("CUBE Top: " + (space.y + cSize.y/2));
            //print("SPACE Bottom: " + (position.y - size.y/2));
            if (space.y + cSize.y/2 == position.y - size.y/2)
                return true;
        }
        return false;
    }

    public void updatePositions(cube c)
    {
        Vector3 size = c.getSize();

        spaces.Remove(c.target);
        print("POSITION: " + c.target + " SIZE: " + size);
            for (float i = (float)Math.Truncate(-size.x/2); i <= size.x/2; i+=1)
                for (float j = (float)Math.Truncate(-size.y/2); j <= size.y/2; j+=1)
                    for(float k = (float)Math.Truncate(-size.z/2); k <= size.z/2; k+=1)
                    {
                
                        Vector3 a = c.target + new Vector3(i,j,k);
                        //Vector3 b = c.target + new Vector3(-i,-j,-k);
                        spaces.Remove(a);
                        //spaces.Remove(b);
                        //print("Removed: "+ a);
                    }
            /*for (float j = 0f; j < size.y/2; j+=1f)
            {
                Vector3 a = c.target + new Vector3(0,j,0);
                Vector3 b = c.target + new Vector3(0,-j,0);
                spaces.Remove(a);
                spaces.Remove(b);
                print("Removed: "+ a + ", " + b);          
            }       
            for (float j = 0f; j < size.z/2; j+=1f)
            {
                Vector3 a = c.target + new Vector3(0,0,j);
                Vector3 b = c.target + new Vector3(0,0,-j);
                spaces.Remove(a);
                spaces.Remove(b);
                print("Removed: "+ a + ", " + b);
            }*/ 
            print(spaces.Count);            
        

    }
    public bool doesCollide(Vector3 target, Vector3 s)
    {
        Vector3 size = s;

        Vector3 position = target;

        for (int i = 0; i < cubes.Count; i++)
        {
            cube otherCube = cubes[i];

            Vector3 collisionRadius = otherCube.getSize();

            if (Vector3.Distance(position,otherCube.target) < Vector3.Distance(collisionRadius/2,size/2))
            {
                //spaces.Remove(position);
                //print("COLLIDE - C1: " + position + " C2: " + otherCube.target);
                return true;
            }
            /*if (Math.Abs(position.y - otherCube.target.y) < collisionRadius.y/2 + size.y/2)
            {
                //spaces.Remove(position);
                //print("COLLIDE - C1: " + position + " C2: " + otherCube.target);
                return true;
            }
            if (Math.Abs(position.z - otherCube.target.z) < collisionRadius.z/2 + size.z/2)
            {
                //spaces.Remove(position);
                //print("COLLIDE - C1: " + position + " C2: " + otherCube.target);
                return true;
            }*/
        }

        return false;
    }

    /*public bool collides(cube c)
    {
        for(int i = 0; i < cubes.Count; i+=1)
        {
            if (c.transform.position == target && cubes[i].transform.position == cubes[i].target)
                if c.OnTriggerStay()

        }

    }*/
    public void printList(int y = 10)
    {
        int i = y;
        for (i = 0; i < spaces.Count; i+=1)
            print(i + ": " + spaces[i].x + " " + spaces[i].y + " " + spaces[i].z);
        print("");
    }


}
