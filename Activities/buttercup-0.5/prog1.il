// Code generated by the buttercup compiler.

.assembly 'buttercup' {}

.assembly extern 'bcuplib' {}

.class public 'ButtercupProgram' extends ['mscorlib']'System'.'Object' {
	.method public static void 'start'() {
		.entrypoint
		.locals init (bool 'b')
		.locals init (int32 'x')
		ldc.i4.1
		ldc.i4.5
		clt
		ldc.i4.0
		and
		ldc.i4.1
		and
		stloc 'b'
		ldc.i4.1
		stloc 'x'
		ldc.i4.1
		brfalse '$000000'
		ldloc 'x'
		call void class ['bcuplib']'Buttercup'.'Utils'::'Print'(int32)
		ldloc 'b'
		call void class ['bcuplib']'Buttercup'.'Utils'::'Print'(bool)
	'$000000':
		ldloc 'b'
		ldc.i4.1
		ldc.i4.2
		clt
		and
		ldc.i4.1
		and
		call void class ['bcuplib']'Buttercup'.'Utils'::'Print'(bool)
		ret
	}
}
