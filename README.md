# Procedural City Generator (Unity Built-in)

This Unity project procedurally generates a stylized city composed of modular buildings with procedural facades.

## Overview
The scene automatically creates a grid of blocks, each filled with buildings that use a procedural material to simulate lit windows at night.  
The result is a simple but expressive cityscape, entirely generated from code.

## Key Features
- Procedural city layout (`CityGenerator.cs`)
- Procedural building facades (`SimpleFacadeGenerator.cs`)
- Camera orbit system (`CameraOrbit.cs`)
- Editable directly in the Unity Editor
- Lightweight — uses **Built-in Render Pipeline**

> Note: The project was built using the **Built-in Pipeline** for simplicity, but it can be easily upgraded to **URP** for improved lighting and post-processing.

## Requirements
- **Unity Version:** 2022.3.50f1 (LTS)
- **Render Pipeline:** Built-in (no URP required)

##  How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/YOURUSERNAME/procedural-city-generator.git
