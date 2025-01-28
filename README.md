# Field_Of_View
 Field_Of_View
 
# 📜 Game Documentation: Field of View 🎮

## 📌 Game Title: Field of View  
![Game Image](https://user-images.githubusercontent.com/62818241/201537367-0af1bf60-2a03-483f-ab7e-f586f662d5e1.png)

## 🎨 Art Style
The game follows a **low-poly minimalist aesthetic** with a **realistic lighting** setup. The color palette is simple, focusing on contrast to highlight enemies and objects in the field of view.

---
## 📖 Game Overview
**Field of View** is a **stealth-based simulation** where the player’s visibility and awareness are determined by a dynamically calculated field of view. The game mechanics focus on **line of sight, detection, and strategic movement** to avoid obstacles and detect enemies.

### 🎯 Objectives
- Detect enemies within a dynamically generated **field of view**.
- Avoid obstacles that block visibility.
- Use stealth mechanics to maneuver through the environment.

---
## 🎮 Game Mechanics
### 🔍 Field of View System
- Players can see only objects within their **view radius and angle**.
- Objects outside the **line of sight** remain hidden.
- View is blocked by obstacles (walls, objects, etc.).

### 🚶 Movement System
- Players **control movement** using the keyboard.
- The camera follows the player with **smooth movement mechanics**.
- Player rotation is controlled via **mouse movement**.

### 👁️ Detection System
- Objects/enemies within the player’s **field of view** are detected.
- Enemies hidden behind obstacles **remain undetected**.
- A visual indicator highlights **detected enemies**.

---
## 🛠️ Script Documentation

### 🎮 **Controller.cs**
Handles **player movement and camera rotation**.

```csharp
public class Controller : MonoBehaviour
{
    public float mouseSensitivity = 10f;
    public float moveSpeed = 6f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }
    
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }
}
```

### 👁️ **FieldOfView.cs**
Handles **player visibility, enemy detection, and field of view rendering**.

```csharp
public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public LayerMask targetMask, obstacleMask;
    private List<Transform> visibleTargets = new List<Transform>();
    
    void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }
    
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        
        foreach (var target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dirToTarget, Vector3.Distance(transform.position, target.transform.position), obstacleMask))
                {
                    visibleTargets.Add(target.transform);
                }
            }
        }
    }
}
```

### 🏗️ **FieldOfViewEditor.cs**
Handles **editor visualization** of the field of view.

```csharp
[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        
        Vector3 viewAngleA = fov.DirFormAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFormAngle(fov.viewAngle / 2, false);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);
        
        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}
```

---
## 🏆 Features & Future Improvements
✅ **Basic enemy detection system**
✅ **Field of view visualization**
✅ **Smooth player movement**
🔜 **AI-based enemy patrol system**
🔜 **Dynamic shadows and lighting effects**
🔜 **Improved UI and feedback mechanisms**

---
## 📌 Conclusion
**Field of View** is a dynamic visibility-based simulation focusing on stealth and awareness. With **real-time enemy detection**, **realistic movement**, and **customizable field of view mechanics**, the game offers an engaging experience. Future updates will introduce **AI enhancements** and **advanced environmental interactions**. 🚀🎮


