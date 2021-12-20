using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientScript : MonoBehaviour
{
    public static ClientScript Instance;
    [SerializeField] private Animator animator;

    [SerializeField] private ClientVersion currentClientVersion;


    [Header("References")]
    [SerializeField] private ClientReferences boyReferences;
    [SerializeField] private ClientReferences girlReferences;
    [SerializeField] private List<ClientVersion> clientVersions = new List<ClientVersion>();

    private ClientVersion versionToTransformTo;


    private void Awake()
    {
        Instance = this;

        //Invoke(nameof(Try), 1);
    }

    [ContextMenu("try")]
    private void Try()
    {
        TransformVersion(0);
    }

    public Animator GetCurrentAnimator()
    {
        return GetComponentInChildren<Animator>();
    }

    public void Animate(CharacterAnims anim)
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        
        animator.SetTrigger($"{(int)anim}");
    }

    public void Animate(string anim)
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        animator.SetTrigger(anim);
    }


    [ContextMenu("Save current prefab state into a new client version")]
    public void SaveCurrentAsNewClientVersion()
    {
        Transform parentToUse;
        bool isMale;

        if (transform.GetChild(0).gameObject.activeInHierarchy && !transform.GetChild(1).gameObject.activeInHierarchy)
        {
            currentClientVersion.gender = ClientGender.male;
            parentToUse = transform.GetChild(0);
            isMale = true;
        }
        else if (transform.GetChild(1).gameObject.activeInHierarchy && !transform.GetChild(0).gameObject.activeInHierarchy)
        {
            currentClientVersion.gender = ClientGender.female;
            parentToUse = transform.GetChild(1);
            isMale = false;
        }   
        else
        {
            Debug.LogError("Client Must only have 1 gender!");
            return;
        }

        currentClientVersion.activatedObjects.Clear();
        currentClientVersion.headBlendshapes.Clear();
        currentClientVersion.bodyBlendshapes.Clear();
        CheckActivatedChildren(parentToUse);

        if (isMale)
        {
            Debug.Log(boyReferences.headSkinnedRenderer.sharedMesh.blendShapeCount);
            for (int i = 0; i < boyReferences.headSkinnedRenderer.sharedMesh.blendShapeCount; i++)
            {
                currentClientVersion.headBlendshapes.Add(boyReferences.headSkinnedRenderer.GetBlendShapeWeight(i));
                Debug.Log(i);
            }

            for (int i = 0; i < boyReferences.bodySkinnedRenderer.sharedMesh.blendShapeCount; i++)
            {
                currentClientVersion.bodyBlendshapes.Add(boyReferences.bodySkinnedRenderer.GetBlendShapeWeight(i));
            }

            currentClientVersion.headTexture = boyReferences.headSkinnedRenderer.sharedMaterial.mainTexture;
            currentClientVersion.bodyTexture = boyReferences.bodySkinnedRenderer.sharedMaterial.mainTexture;
        }
        else
        {
            for (int i = 0; i < girlReferences.headSkinnedRenderer.sharedMesh.blendShapeCount; i++)
            {
                currentClientVersion.headBlendshapes.Add(girlReferences.headSkinnedRenderer.GetBlendShapeWeight(i));
            }

            for (int i = 0; i < girlReferences.bodySkinnedRenderer.sharedMesh.blendShapeCount; i++)
            {
                currentClientVersion.bodyBlendshapes.Add(girlReferences.bodySkinnedRenderer.GetBlendShapeWeight(i));
            }

            currentClientVersion.headTexture = girlReferences.headSkinnedRenderer.sharedMaterial.mainTexture;
            currentClientVersion.bodyTexture = girlReferences.bodySkinnedRenderer.sharedMaterial.mainTexture;
        }

        if (currentClientVersion.name == "")
        {
            Debug.LogError("Please put a name into the new client version!");
            return;
        }

        //var a = currentClientVersion;
        //clientVersions.Add(a);
        //Debug.Log($"{currentClientVersion.name} SAVED AS A NEW CLIENT VERSION!");
    }

    [ContextMenu("Try")]
    public void Try2()
    {
        TransformVersion(0);
    }

    public void TransformVersion(int index)
    {
        StartCoroutine(TransformVersionRoutine(index));
    }

    private IEnumerator TransformVersionRoutine(int index)
    {
        yield return new WaitForSeconds(0);
        versionToTransformTo = clientVersions[index];

        if (versionToTransformTo.gender == ClientGender.male)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            CheckChildrenIfActivated(transform.GetChild(0));

            for (int i = 0; i < versionToTransformTo.bodyBlendshapes.Count; i++)
                boyReferences.bodySkinnedRenderer.SetBlendShapeWeight(i, versionToTransformTo.bodyBlendshapes[i]);

            for (int i = 0; i < versionToTransformTo.headBlendshapes.Count; i++)
                boyReferences.headSkinnedRenderer.SetBlendShapeWeight(i, versionToTransformTo.headBlendshapes[i]);

            boyReferences.bodySkinnedRenderer.sharedMaterial.mainTexture = versionToTransformTo.bodyTexture;
            boyReferences.headSkinnedRenderer.sharedMaterial.mainTexture = versionToTransformTo.headTexture;

        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            CheckChildrenIfActivated(transform.GetChild(1));


            for (int i = 0; i < versionToTransformTo.bodyBlendshapes.Count; i++)
                girlReferences.bodySkinnedRenderer.SetBlendShapeWeight(i, versionToTransformTo.bodyBlendshapes[i]);

            for (int i = 0; i < versionToTransformTo.headBlendshapes.Count; i++)
                girlReferences.headSkinnedRenderer.SetBlendShapeWeight(i, versionToTransformTo.headBlendshapes[i]);

            girlReferences.bodySkinnedRenderer.sharedMaterial.mainTexture = versionToTransformTo.bodyTexture;
            girlReferences.headSkinnedRenderer.sharedMaterial.mainTexture = versionToTransformTo.headTexture;
        }

        animator = GetComponentInChildren<Animator>();
    }
    
    private void ActivateAnim()
    {
        animator.enabled = true;
    }

    private void CheckChildrenIfActivated(Transform parent)
    {
        foreach (Transform t in parent.GetComponentInChildren<Transform>(true))
        {
            t.gameObject.SetActive(versionToTransformTo.activatedObjects.Contains(t.gameObject));

            if (t.childCount > 0)
                CheckChildrenIfActivated(t);
        }
    }

    private void CheckActivatedChildren(Transform parent)
    {
        foreach (Transform t in parent.GetComponentInChildren<Transform>())
        {
            if (t.gameObject.activeInHierarchy)
            {
                currentClientVersion.activatedObjects.Add(t.gameObject);

                CheckActivatedChildren(t);
            }
        }
    }
}

[System.Serializable]
public class ClientReferences
{
    public SkinnedMeshRenderer headSkinnedRenderer;
    public SkinnedMeshRenderer bodySkinnedRenderer;
}

[System.Serializable]
public class ClientVersion
{
    public string name;
    public ClientGender gender;
    public List<GameObject> activatedObjects = new List<GameObject>();
    public List<float> headBlendshapes, bodyBlendshapes = new List<float>();
    public Texture headTexture, bodyTexture;
}

public enum ClientGender
{
    male,
    female
}

public enum ClientVersions
{
    lola,
    lolaPretty,
    lolaUglier,
    femalePoor,
    femaleRich,
    femaleNormal,
    femaleFamous,
    femaleBeard,
    femaleNoBeard,
    femaleFrog,
    femaleNormal2,
    femaleUgly,
    femaleUglyBearded,
    femalePretty,
    femaleOgre,
    femaleOgreToNormal,
    maleOld,
    maleOldUglier,
    maleOldToYounger,
    maleUgly,
    maleUglyToHandsome,
    malePoor,
    maleRich,
    maleHeadless,
    FEMALE_NORMAL1,
    FEMALE_NORMAL2
}