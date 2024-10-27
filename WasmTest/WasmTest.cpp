// WasmTest.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <iostream>
#include <mutex>
#include <emscripten.h>

static int array[4] = { 0,0,0,0 };
static int i = 0;

static std::once_flag flag;
inline static void MaybeInit() {
	std::call_once(flag, [] {array[i] = 100; });
}

#ifdef __cplusplus
#define EXTERN extern "C"
#else
#define EXTERN
#endif

EXTERN EMSCRIPTEN_KEEPALIVE void add(int input) {
	if (i == 4)
		i = 0;
	array[i] += input;
	printf("%d %d %d %d\n", array[0], array[1], array[2], array[3]);
	i++;
}

int main()
{
	MaybeInit();
	add(1);
	std::cout << "Hello World!\n";
}