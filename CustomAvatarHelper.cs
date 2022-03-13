using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomAvatarHelper : MonoBehaviour
{
    private GraspController _graspController = new GraspController();

    [SerializeField]
    private bool _mirrored = true;
    private Transform _leftHand;
    private Transform _rightHand;
    private Quaternion _leftHandRotation;
    private Quaternion _rightHandRotation;
    private Vector3 _leftHandPosition;
    private Vector3 _rightHandPosition;

    [SerializeField]
    private Quaternion _leftHandGrasp = Quaternion.AngleAxis(0, Vector3.forward);
    [SerializeField]
    private Quaternion _rightHandGrasp = Quaternion.AngleAxis(0, Vector3.forward);
    [SerializeField]
    private Quaternion _leftHandThumb = Quaternion.AngleAxis(0, Vector3.forward);
    [SerializeField]
    private Quaternion _rightHandThumb = Quaternion.AngleAxis(0, Vector3.forward);

    private Texture _transparent;
    [SerializeField]
    private bool _syncShader = true;
    private string[] _options = { "Lit Glow", "Unlit Glow", "CellShading" };
    [SerializeField]
    private int _shaderIndex = 0;
    [SerializeField]
    private float _glow = 0.1f;
    [SerializeField]
    private float _ambient = 0.05f;
    [SerializeField]
    private Quaternion _lightDirection = Quaternion.FromToRotation(Vector3.up, new Vector3(-1, -1, 0));
    [SerializeField]


    public bool getMirrored()
    {
        return _mirrored;
    }

    public void setMirrored(bool mirrored)
    {
        _mirrored = mirrored;
    }

    public void setLeftHandRotation(Quaternion rotation)
    {
        _leftHandRotation = rotation;
        _leftHand.rotation = rotation;

        if (_mirrored)
        {
            Quaternion mirroredRotation = new Quaternion();
            mirroredRotation.eulerAngles = new Vector3(rotation.eulerAngles.x, -rotation.eulerAngles.y, rotation.eulerAngles.z + 180);
            _rightHand.rotation = mirroredRotation;
            _rightHandRotation = mirroredRotation;
        }
    }

    public void setRightHandRotation(Quaternion rotation)
    {
        _rightHandRotation = rotation;
        _rightHand.rotation = rotation;
        
        if (_mirrored)
        {
            Quaternion mirroredRotation = new Quaternion();
            mirroredRotation.eulerAngles = new Vector3(rotation.eulerAngles.x, -rotation.eulerAngles.y, rotation.eulerAngles.z + 180);
            _leftHand.rotation = mirroredRotation;
            _leftHandRotation = mirroredRotation;
        }
    }

    public void setLeftHandPosition(Vector3 position)
    {
        _leftHandPosition = position;
        _leftHand.position = position;

        if (_mirrored)
        {
            Vector3 mirroredPosition = new Vector3(-position.x, position.y, position.z);
            _rightHand.position = mirroredPosition;
            _rightHandPosition = mirroredPosition;
        }
    }

    public void setRightHandPosition(Vector3 position)
    {
        _rightHandPosition = position;
        _rightHand.position = position;

        if (_mirrored)
        {
            Vector3 mirroredPosition = new Vector3(-position.x, position.y, position.z);
            _leftHand.position = mirroredPosition;
            _leftHandPosition = mirroredPosition;
        }
    }

    public Quaternion getLeftHandRotation()
    {
        return _leftHandRotation;
    }

    public Quaternion getRightHandRotation()
    {
        return _rightHandRotation;
    }

    public Vector3 getLeftHandPosition()
    {
        return _leftHandPosition;
    }

    public Vector3 getRightHandPosition()
    {
        return _rightHandPosition;
    }

    private void curlHands(Quaternion thumb, Quaternion grasp, bool left)
    {
        float curlFinger = grasp.eulerAngles.z;
        float curlThumb = thumb.eulerAngles.x;

        if (_mirrored)
        {
            _leftHandThumb = thumb;
            _leftHandGrasp = grasp;
            _rightHandThumb = thumb;
            _rightHandGrasp = grasp;

            _graspController.curlHand(curlThumb, left ? curlFinger : curlFinger * -1);
        }
        else if (left)
        {
            _leftHandThumb = thumb;
            _leftHandGrasp = grasp;
            _graspController.curlHandLeft(curlThumb, curlFinger);
        }
        else
        {
            _rightHandThumb = thumb;
            _rightHandGrasp = grasp;
            _graspController.curlHandRight(curlThumb, curlFinger * -1);
        }
    }

    public Quaternion getLeftHandGrasp()
    {
        return _leftHandGrasp;
    }

    public void setLeftHandGrasp(Quaternion grasp)
    {
        curlHands(_leftHandThumb, grasp, true);
    }

    public Quaternion getRightHandGrasp()
    {
        return _rightHandGrasp;
    }

    public void setRightHandGrasp(Quaternion grasp)
    {
        curlHands(_rightHandThumb, grasp, false);
    }

    public Quaternion getLeftHandThumb()
    {
        return _leftHandThumb;
    }

    public void setLeftHandThumb(Quaternion thumb)
    {
        curlHands(thumb, _leftHandGrasp, true);
    }

    public Quaternion getRightHandThumb()
    {
        return _leftHandThumb;
    }

    public void setRightHandThumb(Quaternion thumb)
    {
        _leftHandThumb = thumb;
        curlHands(thumb, _rightHandGrasp, false);
    }

    public void setShaders()
    {
        string shader = _options[_shaderIndex];
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.sharedMaterials)
            {
                if (material.shader.name != "BeatSaber/Transparent")
                {
                    Texture texture = material.mainTexture;
                    Vector2 offset = material.mainTextureOffset;
                    Vector2 scale = material.mainTextureScale;
                    material.shader = Shader.Find(string.Format("BeatSaber/{0}", shader));
                    material.SetTexture("_Tex", texture);
                    material.SetTextureOffset("_Tex", offset);
                    material.SetTextureScale("_Tex", scale);
                    material.SetFloat("_Ambient", _ambient);
                    material.SetFloat("_Glow", _glow);
                    Vector3 rotated = _lightDirection * Vector3.up;
                    material.SetVector("_LightDir", new Vector4(rotated.x, rotated.y, rotated.z, 1));
                }
            }
        }
    }

    public void setTransparent(Texture transparent)
    {
        _transparent = transparent;
    }

    public bool getSyncShader()
    {
        return _syncShader;
    }

    public void setSyncShader(bool syncShader)
    {
        _syncShader = syncShader;
    }

    public string[] getShaderOptions()
    {
        return _options;
    }

    public int getShaderIndex()
    {
        return _shaderIndex;
    }

    public void setShaderIndex(int shaderIndex)
    {
        _shaderIndex = shaderIndex;
    }

    public float getGlow()
    {
        return _glow;
    }

    public void setGlow(float glow)
    {
        _glow = glow;
    }

    public float getAmbient()
    {
        return _ambient;
    }

    public void setAmbient(float ambient)
    {
        _ambient = ambient;
    }

    public Quaternion getLightDirection()
    {
        return _lightDirection;
    }

    public void setLightDirection(Quaternion lightDirection)
    {
        _lightDirection = lightDirection;
    }

    void Start()
    {
        _leftHand = this.gameObject.transform.Find("LeftHand");
        _rightHand = this.gameObject.transform.Find("RightHand");

        if (_leftHand == null || _rightHand == null)
        {
            return;
        }
 
        _leftHandRotation = _leftHand.rotation;
        _rightHandRotation = _rightHand.rotation;

        _leftHandPosition = _leftHand.position;
        _rightHandPosition = _rightHand.position;

        _graspController.setAvatar(GetComponentInChildren<Animator>());
        _graspController.saveBoneTransforms();
    }
}
