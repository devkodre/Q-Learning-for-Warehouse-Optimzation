import matplotlib.pyplot as plt
import pickle

# Sample Q-table data (replace with your actual data)
with open("/Users/devkodre/My project/Assets/qtable3.pickle", "rb") as file:
    qTable = pickle.load(file)

# Function to get the length of the "cubes" list from a Q-table key
def get_cubes_length(key):
    _, cubes = key
    #print(f"{cubes}   {len(cubes)}")
    return len(cubes)

# Initialize a dictionary to store the frequencies
frequencies = {}

# Count the frequencies
for key in qTable.keys():
    cubes_length = get_cubes_length(key)
    if cubes_length not in frequencies:
        frequencies[cubes_length] = 1
    else:
        frequencies[cubes_length] += 1

# Create the x-axis labels and sort them
x_labels = sorted(frequencies.keys())

# Extract the frequencies in the sorted order
sorted_frequencies = [frequencies[x] for x in x_labels]

# Plot the line graph
fig, ax = plt.subplots(figsize=(12, 6))  # Adjust the figure size as needed
ax.plot(x_labels, sorted_frequencies, marker='o')
ax.set_xlabel("Length of 'cubes' list")
ax.set_ylabel("Frequency")
ax.set_title("Frequencies of States in Q-table")

# Rotate x-axis labels and adjust spacing
plt.xticks(rotation=45, ha='right')
plt.subplots_adjust(bottom=0.3)  # Adjust the bottom spacing to prevent label clipping

plt.show()