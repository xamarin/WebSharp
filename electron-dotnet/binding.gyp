##
# Portions Copyright (c) Microsoft Corporation. All rights reserved. 
# 
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#  http://www.apache.org/licenses/LICENSE-2.0  
#
# THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS
# OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION 
# ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR 
# PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
#
# See the Apache Version 2.0 License for specific language governing 
# permissions and limitations under the License.
##
{
  'targets': [
    {
      'variables': {
        'MONO_PKG_CONFIG': '/Library/Frameworks/Mono.framework/Versions/Current/bin/pkg-config',
        'conditions': [
        [
          'OS=="win"',
          {
            'conditions': [
              ['target_arch=="ia32"', {
                'DEFAULT_MONO_ROOT': '<!(echo %MONO_ROOT_X86:"=%)'
              }],
              ['target_arch=="x64"', {
                'DEFAULT_MONO_ROOT': '<!(echo %MONO_ROOT_X64:"=%)'
              }]],
          },
          'OS!="win"',
          {
            'DEFAULT_MONO_ROOT': ''
          }
        ]]
      },
      'target_name': 'websharp_monoclr',
      'win_delay_load_hook': 'false',
      'include_dirs': [
        "<!(node -e \"require('nan')\")"
      ],
      'cflags+': [
        '-DHAVE_NATIVECLR -std=c++11'
      ],
      'xcode_settings': {
        'OTHER_CFLAGS': [
          '-DHAVE_NATIVECLR'
        ]
      },
      'conditions': [
        [
          'OS=="win"',
          {
              'include_dirs+': [
                '<(DEFAULT_MONO_ROOT)\include\mono-2.0'
              ],
              'sources+': [
                  'src/mono/clractioncontext.cpp',
                  'src/mono/clrfunc.cpp',
                  'src/mono/clrfuncinvokecontext.cpp',
                  'src/mono/monoembedding.cpp',
                  'src/mono/task.cpp',
                  'src/mono/dictionary.cpp',
                  'src/mono/nodejsfunc.cpp',
                  'src/mono/nodejsfuncinvokecontext.cpp',
                  'src/mono/utils.cpp',
                  'src/common/v8synchronizationcontext.cpp',
                  'src/common/edge.cpp'
              ]

          },
          {
            'conditions': [
              [
                '"<!((which mono 2>/dev/null) || echo not_found)"!="not_found"',
                {
                  'sources+': [
                    'src/mono/clractioncontext.cpp',
                    'src/mono/clrfunc.cpp',
                    'src/mono/clrfuncinvokecontext.cpp',
                    'src/mono/monoembedding.cpp',
                    'src/mono/task.cpp',
                    'src/mono/dictionary.cpp',
                    'src/mono/nodejsfunc.cpp',
                    'src/mono/nodejsfuncinvokecontext.cpp',
                    'src/mono/utils.cpp',
                    'src/common/utils.cpp',
                    'src/common/v8synchronizationcontext.cpp',
                    'src/common/edge.cpp'
                  ],
                  'conditions': 
                  [
                    [
                      '"<!((pkg-config mono-2 --libs 2>/dev/null) || echo not_found)"!="not_found"',
                      {
                            'include_dirs': [
                              '<!@(pkg-config mono-2, glib-2.0 --cflags-only-I | sed s/-I//g)'
                            ],
                            'link_settings': {
                              'libraries': [
                                '<!@(pkg-config mono-2, glib-2.0 --libs)'
                              ]
                            }
                      },
                      '"<!((pkg-config mono-2 --libs 2>/dev/null) || echo not_found)"=="not_found"',
                      {
                            'include_dirs': [
                              '<!@(<(MONO_PKG_CONFIG) mono-2, glib-2.0 --cflags-only-I | sed s/-I//g)'
                            ],
                            'link_settings': {
                              'libraries': [
                                '<!@(<(MONO_PKG_CONFIG) mono-2, glib-2.0 --libs)'
                              ]
                            }
                      }
                    ]
                  ],
                },
                {
                  'type': 'none'
                }
              ]
            ]
          }
        ]
      ],
      'configurations': {
        'Release': {
          'msvs_settings': {
            'VCCLCompilerTool': {
              # this is out of range and will generate a warning and skip adding RuntimeLibrary property:
              'RuntimeLibrary': -1,
              # this is out of range and will generate a warning and skip adding RuntimeTypeInfo property:
              'RuntimeTypeInfo': -1,
              'BasicRuntimeChecks': -1,
              'ExceptionHandling': '0',
              'AdditionalOptions': [
                '/clr',
                '/wd4506',
				'/DHAVE_NATIVECLR',
                '/DHAVE_MONO'
              ]
            },
            'VCLinkerTool': {
			  'AdditionalDependencies' :
			  [
				'mono-2.0-sgen.lib'
			  ],
			  'AdditionalLibraryDirectories' :
			  [
				'<(DEFAULT_MONO_ROOT)\lib'
			  ],
              'AdditionalOptions': [
                '/ignore:4248'
              ]
            }
          }
        },
        'Debug': {
          'msvs_settings': {
            'VCCLCompilerTool': {
              # this should be /MDd but not being set properly so it is overridden in AdditionalOptions:
              'RuntimeLibrary': 3,
              # this is out of range and will generate a warning and skip adding RuntimeTypeInfo property:
              'RuntimeTypeInfo': -1,
              'BasicRuntimeChecks': -1,
              'ExceptionHandling': '0',
              'AdditionalOptions': [
                '/clr',
                '/wd4506',
                '/DHAVE_NATIVECLR',
                '/DHAVE_MONO',
				'/MDd'
              ]
            },
            'VCLinkerTool': {
			  'AdditionalDependencies' :
			  [
				'mono-2.0-sgen.lib'
			  ],
			  'AdditionalLibraryDirectories' :
			  [
				'<(DEFAULT_MONO_ROOT)\lib'
			  ],
			  'AdditionalOptions': [
                '/ignore:4248'
              ]
            }
          }
        }
      }
    },
    {
      'target_name': 'build_managed',
      'type': 'none',
      'dependencies': [
        'websharp_monoclr'
      ],
      'conditions': [
        [
          'OS=="win"',
          {
			        'conditions': [
                [
                  '"<!(node -e \"require(\'./tools/gyp-whereis.js\')(\'mono.exe\')\")"!="null"',
                  {
                 'actions+': [
                    {
                      'action_name': 'compile_mono_embed',
                      'inputs': [
                        'src/mono/*.cs'
                      ],
                      'outputs': [
                        '$(ConfigurationName)/monoembedding.exe'
                      ],
                      'action': [
                        'csc',
                        '-target:exe',
                        '-out:$(ConfigurationName)/MonoEmbedding.exe',
                        'src/mono/*.cs',
                        'src/common/*.cs'
                      ]
                    }
                  ]                    
				          }
			          ]
			        ]            
          },
          {
            'conditions': [
              [
                '"<!((which mono 2>/dev/null) || echo not_found)"!="not_found"',
                {
                  'actions+': [
                    {
                      'action_name': 'compile_mono_embed',
                      'inputs': [
                        'src/mono/*.cs'
                      ],
                      'outputs': [
                        'build/$(BUILDTYPE)/monoembedding.exe'
                      ],
                      'action': [
                        'mcs',
                        '-sdk:4.5',
                        '-target:exe',
                        '-out:build/$(BUILDTYPE)/MonoEmbedding.exe',
                        'src/mono/*.cs',
                        'src/common/*.cs'
                      ]
                    }
                  ]
                }
              ]
            ]
          }
        ]
      ]
    }
  ]
}