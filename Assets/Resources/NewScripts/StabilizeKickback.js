var returnSpeed : float = 2.0;

function Update () {
	transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
}