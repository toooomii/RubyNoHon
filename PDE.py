import numpy as np

def MakeAMatrix(t, n, mu, sigma, r, d):
    A = np.zeros((n, n))
    for i in range(n):
        for j in range(n):
            if i == j:
                A[i, j] = - sigma[i] ** 2 / d ** 2 - r[i]
            elif i == j + 1:
                A[i, j] = mu[i] / (2 * d) + sigma[i] ** 2 / d ** 2
            elif i == j - 1 and i > 0:
                A[i, j] = - mu[i] / d + sigma[i] ** 2 / d ** 2
    return A

def IterateThetaScheme(A, theta, d):
    n = int(np.sqrt(np.size(A)))

    leftMatrix = np.eye(n) - theta * d * A
    rightMatrix = np.eye(n) + (1 - theta) * d * A

    return np.linalg.inv(leftMatrix)

def main():
    n = 100
    T = 100
    theta = 1 / 2
    r = 0.01 * np.ones((n, T))
    sigma = 0.03 * np.ones((n, T))
    mu = 0.03 * np.ones((n, T))
    d = 0.01
    K = 0.1

    V = np.zeros((n, T))
    V[:, T - 1] = np.array([np.max(np.exp((ni - n / 2) * d) - K, 0) for ni in range(n)])

    for t in range(T - 1, 0, - 1):
        V[:, t - 1] = np.dot(IterateThetaScheme(MakeAMatrix(t, n, mu[:, t], sigma[:, t], r[:, t], d), theta, d), V[:, t])

    print(V[:, 0])
if __name__ == '__main__':
    main()