using System;
using UnityEngine;

public class FoVSystem : MonoBehaviour
{
    [SerializeField]    private     float       _viewRadius;
    [SerializeField]    private     float       _viewAngle;
                        private     GameObject  _target;
    [SerializeField]    private     LayerMask   _wallLayer;
                        private     GameObject  _myOwner;

    private void Awake()
    {
        if (_myOwner == null) _myOwner = this.gameObject;
    }

    #region MainMethod
    /// <summary>
    /// In case of detection of the target, the method of option A will be executed.
    /// Otherwise, the method of option B will be executed.
    /// </summary>
    /// <param name="myOptionA"></param>
    /// <param name="myOptionB"></param>
    public void FieldOfView(Action myOptionA, Action myOptionB)
    {
        if (_target == null) return;

        Vector3 dir = _target.transform.position - transform.position;
        if (dir.magnitude > _viewRadius)
        {
            myOptionB();
            return;
        }

        if (Vector3.Angle(transform.forward, dir) <= _viewAngle / 2)
        {
            if (!Physics.Raycast(transform.position, dir, out RaycastHit hit, dir.magnitude, _wallLayer))
            {
                Debug.DrawLine(transform.position, _target.transform.position, Color.red);
                _target.gameObject.GetComponent<Renderer>().material.color = Color.red;
                myOptionA();
            }
            else
            {
                Debug.DrawLine(transform.position, hit.point, Color.white);
                myOptionB();
            }
        }
        else
        {
            _target.gameObject.GetComponent<Renderer>().material.color = Color.white;
            myOptionB();
        }
    }
    #endregion

    #region Increase Method
    /// <summary>
    /// You can increase the radius and the angle, as long as you are the gameobject owner of this component.
    /// </summary>
    /// <param name="subjectThatCallingThisFunction">The GameObject owner of this componect</param>
    /// <param name="increaseRadius"></param>
    /// <param name="increaseAngle"></param>
    public void IncreaseFieldOfView(GameObject subjectThatCallingThisFunction, float increaseRadius, float increaseAngle)
    {
        if (subjectThatCallingThisFunction != _myOwner) return;
        else
        {
            _viewRadius += increaseRadius;
            _viewAngle += increaseAngle;
        }
    }
    #endregion

    #region Decrease Method
    /// <summary>
    /// You can decrease the radius and the angle, as long as you are the gameobject owner of this component.
    /// </summary>
    /// <param name="subjectThatCallingThisFunction"></param>
    /// <param name="decreaseRadius"></param>
    /// <param name="decreaseAngle"></param>
    public void DecreaseFieldOfView(GameObject subjectThatCallingThisFunction, float decreaseRadius, float decreaseAngle)
    {
        if (subjectThatCallingThisFunction != _myOwner) return;
        else
        {
            _viewRadius -= decreaseRadius;
            _viewAngle -= decreaseAngle;
        }
    }
    #endregion

    #region Set Target Method
    /// <summary>
    /// You can set the target gameobject, as long as you are the gameobject owner of this component.
    /// </summary>
    /// <param name="subjectThatCallingThisFunction"></param>
    /// <param name="target"></param>
    public void SetTheTarget(GameObject subjectThatCallingThisFunction, GameObject target)
    {
        if (subjectThatCallingThisFunction != _myOwner) return;
        else _target = target;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Vector3 lineA = GetVectorFromAngle(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-_viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * _viewRadius);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    #endregion
}