import random
import math
import time
import pickle
import os

randomSizes = [1,3]
spaces = []
#from cube datastruct
cubes = []
S = 6
# Check if the file exists
if os.path.exists("/Users/devkodre/My project/Assets/qtable3.pickle"):
    # Load qTable from the saved file
    with open("/Users/devkodre/My project/Assets/qtable3.pickle", "rb") as file:
        qTable = pickle.load(file)
else:
    qTable = {} 
#with open("qtable.pickle", "rb") as file:
    #qTable = pickle.load(file)


target = []

indexToPos = []
#CONSTANTS
epsilon = 0.75

#each state is represented by the cube size and the cubes on the field
#values will be represented by q-values for each move
#                 pos:(0,0,0)
#       Q(size,cubes): [0,0,0,0,0,0,0,0,...]
#           ...
#
#




def updatePositions(cube):
    size = cube[1]

    target = cube[0]
    try:
        spaces.remove(target)
    except:
        print(cube)
        print()
        print(cubes)
        print()
        print(spaces)
        SystemExit
    #print("POSITION:", target, "SIZE:", size)

    for i in range(int(-size[0] / 2), int(size[0] / 2) + 1):
        for j in range(int(-size[1] / 2), int(size[1] / 2) + 1):
            for k in range(int(-size[2] / 2), int(size[2] / 2) + 1):
                a = [target[0] + i, target[1] + j, target[2] + k]
                if a in spaces:
                    spaces.remove(a)
                # print("Removed:", a)
    

    #print(len(spaces))

def validSpaces(size):
    valid_spaces_list = []

    for space in spaces:
        flag = False

        # Check if the prism would fit within the boundaries of the warehouse space
        for x in range(int(-size[0] / 2), int(size[0] / 2)):
            if space[0] + x < 0 or space[0] + x >= S:
                flag = True
                break

        if flag:
            continue

        for y in range(int(-size[1] / 2), int(size[1] / 2)):
            if space[1] + y < 0 or space[1] + y >= S:
                flag = True
                break

        if flag:
            continue

        for z in range(int(-size[2] / 2), int(size[2] / 2)):
            if space[2] + z < 0 or space[2] + z >= S:
                flag = True
                break

        if flag:
            continue

        # Check if the space is available for the prism to be placed without overlapping with other prisms
        for x in range(int(-size[0] / 2), int(size[0] / 2) + 1):
            for y in range(int(-size[1] / 2), int(size[1] / 2) + 1):
                for z in range(int(-size[2] / 2), int(size[2] / 2) + 1):
                    a = [space[0] + x, space[1] + y, space[2] + z]
                    if a not in spaces:
                        flag = True
                        break
                if flag:
                    break
            if flag:
                break

        if flag: continue
        valid_spaces_list.append(space)

    return valid_spaces_list

def isOver(position, size, cube):
    space = cube[0]
    c_size = cube[1]

    # First check if the position is within the x and z of the cube it's gonna be on
    if (space[0] - c_size[0] / 2) < position[0] < (space[0] + c_size[0] / 2) and (space[2] - c_size[2] / 2) < position[2] < (space[2] + c_size[2] / 2):

        # Check if the top of the cube == the bottom of the position and size
        if space[1] + c_size[1] / 2 == position[1] - size[1] / 2:
            return True
    return False

def getLegals(cubes,size):
    valids = validSpaces(size)
    if len(valids) == 0:
        #print(f"no spaces for package size: {size}")
        return [-25,-25,-25]
    
    legals = []
    for a in valids:
        bottom = a[1] - int(size[1]/2)
        if bottom == 0:
            legals.append(a)
            continue
        for c in cubes:
            if isOver(a,size,c):
                legals.append(a)
                break

    """if legals:
        i = legals[0] #Change this for some method of exploitation vs exploration
        return i
    else: 
        print(f"no stable spaces for package size: {size}")
        return [-25,-25,-25]"""
    
    if legals: return legals
    return [-25,-25,-25]
        




def getPolicy(state, qTable, legals):
    thresh = random.random()
    if thresh <= epsilon:
        return random.choice(legals)
    else: 
        bestPos = max(qTable[state], key=lambda key: qTable[state][key])
        return [bestPos[0], bestPos[1], bestPos[2]]
def isBordering(cube1_pos, cube1_size, cube2_pos, cube2_size):
    """
    Check if cube1 is bordering cube2.
    
    Args:
        cube1_pos (tuple): Position of cube1 (x, y, z).
        cube1_size (tuple): Size of cube1 (width, height, depth).
        cube2_pos (tuple): Position of cube2 (x, y, z).
        cube2_size (tuple): Size of cube2 (width, height, depth).
    
    Returns:
        bool: True if cube1 is bordering cube2, False otherwise.
    """
    # Calculate boundaries of cube1
    cube1_x_min = cube1_pos[0] - cube1_size[0] // 2
    cube1_x_max = cube1_pos[0] + cube1_size[0] // 2
    cube1_y_min = cube1_pos[1] - cube1_size[1] // 2
    cube1_y_max = cube1_pos[1] + cube1_size[1] // 2
    cube1_z_min = cube1_pos[2] - cube1_size[2] // 2
    cube1_z_max = cube1_pos[2] + cube1_size[2] // 2
    
    # Calculate boundaries of cube2
    cube2_x_min = cube2_pos[0] - cube2_size[0] // 2
    cube2_x_max = cube2_pos[0] + cube2_size[0] // 2
    cube2_y_min = cube2_pos[1] - cube2_size[1] // 2
    cube2_y_max = cube2_pos[1] + cube2_size[1] // 2
    cube2_z_min = cube2_pos[2] - cube2_size[2] // 2
    cube2_z_max = cube2_pos[2] + cube2_size[2] // 2
    
    # Check if cubes are bordering in any dimension
    x_bordering = cube1_x_min == cube2_x_max or cube1_x_max == cube2_x_min
    y_bordering = cube1_y_min == cube2_y_max or cube1_y_max == cube2_y_min
    z_bordering = cube1_z_min == cube2_z_max or cube1_z_max == cube2_z_min
    
    # Return True if bordering in any dimension
    return x_bordering and y_bordering and z_bordering

def evaluate(policy, currentSize):
    # value based on exposed area
    surroundCount = 0
    for cube in cubes:
        target, size = cube
        if isBordering(target,size,policy,currentSize): surroundCount += 1
        for i in range(3):
            if policy[i] - currentSize[i] == 0 or policy[i] + currentSize[i] == S:
                surroundCount += 1 

    if surroundCount == 6:
        surroundCount = -1
    return surroundCount
    
def getState(size, cubes):
    if cubes:
        cubes.sort()
        c = "".join(str(a) for a in cubes)
    else: c = ""
    return ("".join(str(a) for a in size), c)

def maxNext(cubes):
    m = -10
    stateFlag = False
    for x in randomSizes:
        for y in randomSizes:
            for z in randomSizes:
                size = [x,y,z]
                nextState = getState(size, cubes)
                if nextState in qTable:
                    stateFlag = True
                    if len(qTable[nextState]) == 0:
                        continue
                    p = max(qTable[nextState], key=lambda key: qTable[nextState][key])
                    ma = qTable[nextState][p]
                    if  ma > m:
                        m = ma 
    if not stateFlag: return 0
    return m

def updateQTable(legals, state, reward, cubes, policy, lr = 0.05, discount = 0.95):
    p = (policy[0], policy[1], policy[2])
    try:
        q = qTable[state][p]
    except:
        print("ERROR:")
        print(state)
        print()
        print(p)
        print()
        print(qTable[state])
        print()
        print(legals)
        print(cubes)
        with open("/Users/devkodre/My project/Assets/qtable3.pickle", "w") as file:
            pickle.dump(qTable, file)
            file.close()

    d = discount*maxNext(cubes)
    qTable[state][p] = q + lr*(reward + d - q)

def printStats(flesh = False):
    m = -99
    place = ""
    pos = ()
    for state in qTable:
        if len(qTable[state]) == 0: continue
        p = max(qTable[state], key=lambda key: qTable[state][key])
        ma = qTable[state][p]
        if ma > m:
            place = state
            pos = p
            m = ma
        
        if flesh:
            posVal = set()
            count = 0
            for po in qTable[state]:
                if qTable[state][po] != 0:
                    count+=1
                    posVal.add((po,qTable[state][po]))
            if count > 2:
                print(f"fleshed State for size {state[0]}: {posVal}")

    keys = [key for key in qTable]
    keys.sort(key = lambda x: x[1])        
    print()
    #print(state)
    print(pos)
    print(qTable[place][pos])
    


t = time.time()
with open("/Users/devkodre/My project/Assets/cubes.txt", "w") as file:
    while time.time() - t < 3600:
        print(time.time() - t)
        for i in range(10000):
            #if i != 0 and i%10 == 0 and epsilon >= 0.1:
                #epsilon *= 0.9
            epsilon = 0.1
            if i != 0 and i%100 == 0:
                print(i)
            cubes = []
            spaces = []
            indexToPos = []
            for x in range(S):
                for y in range(S):
                    for z in range(S):
                        spaces.append([x,y,z])
                        indexToPos.append([x,y,z])
            
            
            while len(spaces) != 0:
                lCount = 0
                currentSize = [random.choice(randomSizes), random.choice(randomSizes), random.choice(randomSizes)]
                state = getState(currentSize, cubes)
                legals = getLegals(cubes, currentSize)
                if  legals == [-25,-25,-25]:
                    if currentSize[0]*currentSize[1]*currentSize[2] < len(spaces): 
                        continue
                    #printStats()
                    break
                if state not in qTable:
                    qTable[state] = {}
                for pos in legals:
                    if (pos[0],pos[1],pos[2]) not in qTable[state]:
                        qTable[state][(pos[0],pos[1],pos[2])] = 0  # create new entry for state and action
                #get Legals
                #if legals == [-25,-25,-25] and spaces: break #for debugging add something to return low value for not being able to fill up space


                #print(str(lCount) + " " + str(len(legals)))
                policy = getPolicy(state,qTable,legals) # get move
                updatePositions((policy,currentSize)) # updates spaces
                cubes.append((policy,currentSize)) # changes state\
                reward = evaluate(policy,currentSize)
                updateQTable(legals, state, reward, cubes, policy)
                #file.write(f"{policy}, {currentSize}\n") #print to file
        with open("/Users/devkodre/My project/Assets/qtable3.pickle", "wb") as f2:
            pickle.dump(qTable, f2)
            f2.close()
            #evaluate move
            #update q table

file.close()
printStats(True)

with open("/Users/devkodre/My project/Assets/qtable3.pickle", "wb") as file:
    pickle.dump(qTable, file)
    file.close()
#print(cubes)
print(f"Time: {time.time()-t}")







