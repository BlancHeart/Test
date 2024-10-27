const keyBase64: string = "vT3QYrkDx8FkAZ33OLFzC7RvFz/yHr4uoYuwGg7I58A=";

async function importKey(key: string) {
    console.log(`importing key ${key}`);
    return await window.crypto.subtle.importKey(
        "raw",
        Uint8Array.from(atob(key), c => c.charCodeAt(0)),
        "AES-GCM",  // Changed from AES-CBC to AES-GCM
        true,
        ['encrypt', 'decrypt']
    );
}

function arrayBufferToBase64(bytes: ArrayBuffer) {
    const uint8Array = new Uint8Array(bytes);
    let base64String = btoa(String.fromCharCode(...uint8Array));
    return base64String;
}

function base64ToArrayBuffer(base64: string) {
    const binaryString = atob(base64);
    const length = binaryString.length;
    const buffer = new ArrayBuffer(length);
    const view = new Uint8Array(buffer);
    for (let i = 0; i < length; i++) {
        view[i] = binaryString.charCodeAt(i);
    }
    return buffer;
}

async function encryptMessage(plainText: string, key: CryptoKey) {
    const encoded = new TextEncoder().encode(plainText);
    // iv will be needed for decryption
    const iv = window.crypto.getRandomValues(new Uint8Array(12));//or 16?
    const encryptedBuffer = await window.crypto.subtle.encrypt(
        { name: "AES-GCM", iv: iv },
        key,
        encoded
    );
    // Convert encrypted data to base64 string
    const encryptedData = arrayBufferToBase64(encryptedBuffer);
    const ivBase64 = arrayBufferToBase64(iv);

    // Return the IV and encrypted data in base64
    return {
        iv: ivBase64,
        encryptedData: encryptedData,
    };
}

export async function exe() {
    const key = await importKey(keyBase64);
    const res = await encryptMessage("I love you!", key);
    return res.encryptedData;
}
