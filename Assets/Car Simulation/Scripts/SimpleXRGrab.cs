using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleXRGrab : MonoBehaviour
{
    public XRController controller; // Reference to the XRController component
    private GameObject grabbedObject = null; // The object currently grabbed
    private Rigidbody grabbedObjectRb = null; // Rigidbody of the grabbed object
    public XRNode inputSource; // The XR node for the controller (Left or Right)

    private void Update()
    {
        // Use XR controller input for grabbing and releasing
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        if (device.isValid)
        {
            bool triggerPressed;
            device.TryGetFeatureValue(CommonUsages.gripButton, out triggerPressed);

            if (triggerPressed && grabbedObject == null)
            {
                GrabObject();
            }
            else if (!triggerPressed && grabbedObject != null)
            {
                ReleaseObject();
            }
        }
    }

    // Public function to grab an object
    public void GrabObject()
    {
        if (grabbedObject == null)
        {
            // Cast a sphere to detect grabbable objects nearby
            Collider[] colliders = Physics.OverlapSphere(controller.transform.position, 0.1f);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Grabbable")) // Ensure the object is tagged correctly
                {
                    grabbedObject = collider.gameObject;
                    grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();

                    if (grabbedObjectRb != null)
                    {
                        grabbedObjectRb.isKinematic = true; // Disable physics while grabbing
                    }

                    grabbedObject.transform.SetParent(controller.transform); // Attach to controller
                    break;
                }
            }
        }
    }

    // Public function to release the object
    public void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null); // Detach from controller

            if (grabbedObjectRb != null)
            {
                grabbedObjectRb.isKinematic = false; // Re-enable physics
                grabbedObjectRb.velocity = controller.GetComponent<Rigidbody>().velocity; // Apply controller velocity
                grabbedObjectRb.angularVelocity = controller.GetComponent<Rigidbody>().angularVelocity; // Apply controller angular velocity
            }

            grabbedObject = null;
            grabbedObjectRb = null;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the grab range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(controller.transform.position, 0.1f);
    }
}
