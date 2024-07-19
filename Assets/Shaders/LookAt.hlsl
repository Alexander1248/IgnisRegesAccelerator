float4x4 CreateLookAtMatrix_float(float3 position, float3 target, float3 up)
{
    float3 forward = normalize(target - position);
    float3 right = normalize(cross(up, forward));
    float3 upVector = cross(forward, right);

    float4x4 matrix;
    matrix[0] = float4(right, 0.0);
    matrix[1] = float4(upVector, 0.0);
    matrix[2] = float4(forward, 0.0);
    matrix[3] = float4(position, 1.0);

    return matrix;
}